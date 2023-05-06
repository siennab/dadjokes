using System;
namespace DadJokes.Models
{
	public class DadJoke
	{
		public DadJoke(string id, string joke)
		{
			Id = id;
			Joke = joke;
		}

		public string Id { get; set; }

		public string Joke { get; set; }
	}
}

