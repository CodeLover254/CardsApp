namespace CardsApp.Application.Interfaces;

public interface IAppSetupService
{
    Task MigrateDatabase();
    Task SeedDefaultUsers();
}