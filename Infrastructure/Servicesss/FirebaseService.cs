using Application.Repositories;
using Application.Services.FirebaseService;
using Application.Services.FirebaseService.DTOs;
using Domain.Entities;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class FirebaseService : IFirebaseService
    {
        
        private readonly IGenericRepository<FirebaseToken> _tokenRepository;

        public FirebaseService(IGenericRepository<FirebaseToken> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task SendAsync(List<SendFirebaseRequest> requests)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

            var allTokens = requests.SelectMany(r => r.Tokens).Distinct().ToList();

            var allTokensDict = (await _tokenRepository.GetAll()
                .Where(x => allTokens.Contains(x.Token))
                .ToListAsync())
                .ToDictionary(t => t.Token, t => t);

            foreach (var request in requests)
            {
                const int batchSize = 500;

                for (int i = 0; i < request.Tokens.Count; i += batchSize)
                {
                    var batchTokens = request.Tokens.Skip(i).Take(batchSize).ToList();

                    var multicastMessage = new MulticastMessage()
                    {
                        Tokens = batchTokens,
                        Notification = new FirebaseAdmin.Messaging.Notification()
                        {
                            Title = request.Title,
                            Body = request.Body
                        },
                        Data = request.Data
                    };

                    var response = await messaging.SendEachForMulticastAsync(multicastMessage);
                }
            }
            await _tokenRepository.SaveChangesAsync();
        }
    }
}