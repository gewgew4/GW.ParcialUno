using Domain;
using Infrastructure.Repo.Interfaces;

namespace Infrastructure.Repo;

public class PrintJobRepo(PContext context) : GenericRepo<PrintJob>(context), IPrintJobRepo;
