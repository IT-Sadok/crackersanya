namespace LibraryApp.Interfaces;

public interface IBatchOperationProcessor
{
    Task RunSimulationAsync(int numberOfOperations = 50);
}