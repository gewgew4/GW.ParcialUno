using Application.Exceptions;
using Application.Messages;
using Domain;
using Domain.Enums;
using Infrastructure.Kafka;
using Infrastructure.Repo.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Commands;

public class CreatePrintJobCommandHandler : IRequestHandler<CreatePrintJobCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly ILogger<CreatePrintJobCommandHandler> _logger;

    public CreatePrintJobCommandHandler(IUnitOfWork unitOfWork, IKafkaProducer kafkaProducer, ILogger<CreatePrintJobCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreatePrintJobCommand request, CancellationToken cancellationToken)
    {
        var document = await _unitOfWork.DocumentRepo.GetById(request.DocumentId);
        CheckDocument(request, document);

        var printJob = new PrintJob(request.DocumentId, request.Priority);

        await _unitOfWork.PrintJobRepo.Add(printJob);
        await _unitOfWork.SaveAsync();

        var kafkaMessage = CreatePrintJobMessage(request, printJob);

        var messageJson = JsonSerializer.Serialize(kafkaMessage);
        var messageSent = await _kafkaProducer.ProduceAsync("print-jobs", messageJson);
        if (messageSent)
        {
            printJob.Queue();
            await _unitOfWork.PrintJobRepo.Update(printJob);
            await _unitOfWork.SaveAsync();
        }
        else
        {
            throw new InvalidException("Error sending message to Kafka");
        }

        return printJob.Id;
    }

    /// <summary>
    /// Validations on document
    /// </summary>
    /// <param name="request"></param>
    /// <param name="document"></param>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="InvalidException"></exception>
    private void CheckDocument(CreatePrintJobCommand request, Document document)
    {
        if (document == null)
        {
            var message = $"Document with ID {request.DocumentId} not found.";
            _logger.LogError(message);
            throw new NotFoundException(message);
        }

        if (document.Status == DocumentStatus.Printed)
        {
            var message = $"Document with ID {request.DocumentId} already printed.";
            _logger.LogError(message);
            throw new InvalidException(message);
        }
    }

    /// <summary>
    /// Create Kafka message
    /// </summary>
    /// <param name="request"></param>
    /// <param name="printJob"></param>
    /// <returns></returns>
    private static PrintJobMessage CreatePrintJobMessage(CreatePrintJobCommand request, PrintJob printJob)
    {
        return new PrintJobMessage
        {
            JobId = printJob.Id,
            DocumentId = printJob.DocumentId,
            Priority = request.Priority
        };
    }
}