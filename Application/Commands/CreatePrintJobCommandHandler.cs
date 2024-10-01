using Application.Exceptions;
using Common.Messages;
using Domain;
using Domain.Enums;
using Infrastructure.Kafka;
using Infrastructure.Repo.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Commands;

public class CreatePrintJobCommandHandler(IUnitOfWork unitOfWork,
    IKafkaProducer kafkaProducer,
    ILogger<CreatePrintJobCommandHandler> logger) : IRequestHandler<CreatePrintJobCommand, Guid>
{
    public async Task<Guid> Handle(CreatePrintJobCommand request, CancellationToken cancellationToken)
    {
        var document = await unitOfWork.DocumentRepo.GetById(request.DocumentId);
        CheckDocument(request, document);

        var printJob = new PrintJob(request.DocumentId, request.Priority);

        await unitOfWork.PrintJobRepo.Add(printJob);
        await unitOfWork.SaveAsync();

        var kafkaMessage = CreatePrintJobMessage(request, printJob, document);

        var messageJson = JsonSerializer.Serialize(kafkaMessage);
        
        var messageSent = await kafkaProducer.ProduceAsync("print-jobs", messageJson, request.Priority);
        if (messageSent)
        {
            printJob.Queue();
            await unitOfWork.PrintJobRepo.Update(printJob);
            await unitOfWork.SaveAsync();
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
            logger.LogError(message);
            throw new NotFoundException(message);
        }

        if (document.Status == DocumentStatus.Printed)
        {
            var message = $"Document with ID {request.DocumentId} already printed.";
            logger.LogError(message);
            throw new InvalidException(message);
        }
    }

    /// <summary>
    /// Create Kafka message
    /// </summary>
    /// <param name="request"></param>
    /// <param name="printJob"></param>
    /// <returns></returns>
    private static PrintJobMessage CreatePrintJobMessage(CreatePrintJobCommand request, PrintJob printJob, Document document )
    {
        return new PrintJobMessage
        {
            Content = document.Content,
            CreatedAt = printJob.CreatedAt,
            DocumentId = printJob.DocumentId,
            DocumentName = document.Name,
            JobId = printJob.Id,
            Priority = request.Priority
        };
    }
}