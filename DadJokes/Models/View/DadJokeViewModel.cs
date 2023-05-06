namespace DadJokes.Models
{
    public class DadJokeViewModel<T>
    {
        public DadJokeViewModel()
        {
        }

        public T? Result { get; set; }

        public bool HasError { get; set; } = false;

        public string? ErrorMessage { get; set; }

    }
}

