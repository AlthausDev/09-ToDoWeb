using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared;
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

        public IEnumerable<Chore> GetAllAdmin()
        {
            return choreRepository.GetAllAdmin();
        }

        public IEnumerable<Chore> GetAll()
        {
            return choreRepository.GetAll();
        }

        public Chore GetById(int id)
        {
            return choreRepository.GetById(id);
        }

        public void Add(Chore chore)
        {
            choreRepository.Add(chore);
        }

        public void Update(Chore chore)
        {
            choreRepository.Update(chore);
        }

        public void Remove(int id)
        {
            choreRepository.Remove(id);
        }
              
        public void LogicRemove(int id)
        {
            choreRepository.LogicRemove(id);
        }
    }
}
