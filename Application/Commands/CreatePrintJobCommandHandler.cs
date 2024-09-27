using Application.Exceptions;
using Application.Messages;
using Domain;
using Infrastructure.Kafka;
using Infrastructure.Repo.Interfaces;
using MediatR;
using System.Text.Json;

namespace Application.Commands;

public class CreatePrintJobCommandHandler : IRequestHandler<CreatePrintJobCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKafkaProducer _kafkaProducer;

    public CreatePrintJobCommandHandler(IUnitOfWork unitOfWork, IKafkaProducer kafkaProducer)
    {
        _unitOfWork = unitOfWork;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<Guid> Handle(CreatePrintJobCommand request, CancellationToken cancellationToken)
    {
        var document = await _unitOfWork.DocumentRepo.GetById(request.DocumentId);
        if (document == null)
            throw new NotFoundException($"Document with ID {request.DocumentId} not found.");

        var printJob = new PrintJob(request.DocumentId);

        await _unitOfWork.PrintJobRepo.Add(printJob);
        await _unitOfWork.SaveAsync();

        // Crear y enviar mensaje a Kafka
        var kafkaMessage = new PrintJobMessage
        {
            JobId = printJob.Id,
            DocumentId = printJob.DocumentId,
            Priority = document.Priority
        };

        string messageJson = JsonSerializer.Serialize(kafkaMessage);
        await _kafkaProducer.ProduceAsync("print-jobs", messageJson);

        return printJob.Id;
    }
}