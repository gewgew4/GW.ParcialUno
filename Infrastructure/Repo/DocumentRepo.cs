using Domain;
using Infrastructure.Repo.Interfaces;

namespace Infrastructure.Repo;

public class DocumentRepo(PContext context) : GenericRepo<Document>(context), IDocumentRepo;
