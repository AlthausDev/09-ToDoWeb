using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared;
using System.Data.Common;
using TODO_V2.Server.Utils;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Impl
{
    public class ChoreService : IChoreService
    {
        private IChoreRepository choreRepository;

        public ChoreService(IChoreRepository choreRepository)
        {
            this.choreRepository = choreRepository;
        }

        public Task<Chore> Add(Chore chore)
        {
            return choreRepository.Add(chore);
        }

        public Task<Chore> Update(Chore chore)
        {           
            return choreRepository.Update(chore);
        }

        public void Delete(int choreId)
        {
            choreRepository.Delete(choreId);
        }

        public void LogicDelete(int choreId)
        {
            choreRepository.LogicDelete(choreId);
        }

        public Task<IEnumerable<Chore>> GetAll(GetRequest<Chore>? request)
        {
            return choreRepository.GetAll(request);
        }

        public Task<IEnumerable<Chore>> GetAllLogic(GetRequest<Chore>? request)
        {
            return choreRepository.GetAllLogic(request);
        }

        public Chore GetById(int choreId)
        {  
            return choreRepository.GetById(choreId).Result;
        }

    }
}
