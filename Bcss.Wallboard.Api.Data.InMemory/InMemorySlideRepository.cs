﻿using System.Collections.Concurrent;
using System.Threading.Tasks;
using Bcss.Wallboard.Api.Data.Entities;

namespace Bcss.Wallboard.Api.Data.InMemory
{
    /// <summary>
    /// An In-memory repository implementation used for local development and testing.
    /// </summary>
    public class InMemorySlideRepository : ISlideReader, ISlideWriter
    {
        private readonly ConcurrentDictionary<int, Slide> _storage = new ConcurrentDictionary<int, Slide>();

        public InMemorySlideRepository()
        {
            SeedData();
        }

        private void SeedData()
        {
            _storage.GetOrAdd(1, new Slide
            {
                Id = 1,
                Name = "Sample",
                Content =
                    "<h1>My Sample Slide</h1><p>This is a sample slide containing some simple html as an example of what type of content a slide could have.</p>"
            });
        }

        public Task<Slide> GetSlideAsync(int slideId)
        {
            return Task.FromResult(_storage.ContainsKey(slideId) ? _storage[slideId] : default);
        }

        public Task<Slide> CreateSlideAsync(string name, string content)
        {
            var id = GetNextKey();
            var slide = new Slide
            {
                Id = id,
                Name = name,
                Content = content
            };

            _storage[id] = slide;

            return Task.FromResult(slide);
        }

        public Task<bool> DeleteSlideAsync(int slideId)
        {
            if (!_storage.ContainsKey(slideId))
            {
                return Task.FromResult(true);
            }

            var didRemove = _storage.TryRemove(slideId, out _);
            return Task.FromResult(didRemove);
        }

        private int GetNextKey()
        {
            var result = 1;

            foreach(var (k, _) in _storage)
            {
                if (k >= result)
                {
                    result++;
                }
            }

            return result;
        }
    }
}
