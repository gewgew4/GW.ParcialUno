using Domain;
using Infrastructure.Repo.Interfaces;

namespace Infrastructure.Repo;

public class PrintResultRepo(PContext context) : GenericRepo<PrintResult>(context), IPrintResultRepo;
