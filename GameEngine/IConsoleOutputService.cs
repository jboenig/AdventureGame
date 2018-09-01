namespace AdventureGameEngine
{
    public interface IConsoleOutputService
    {
        void Write(string msg);
        void Write(char c);
        void WriteLine();
        void WriteLine(string msg);
        void Clear();
    }
}
