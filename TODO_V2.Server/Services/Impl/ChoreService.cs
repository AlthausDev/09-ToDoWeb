//using TODO_V2.Server.Repository.Interfaces;
//using TODO_V2.Server.Services.Interfaces;
//using TODO_V2.Shared;
//using System.Data.Common;
//using TODO_V2.Server.Utils;
//using TODO_V2.Shared.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace TODO_V2.Server.Services.Impl
//{
//    public class ChoreService : IChoreService
//    {
//        private IChoreRepository choreRepository;

//        public ChoreService(IChoreRepository choreRepository)
//        {
//            this.choreRepository = choreRepository;
//        }

//        public Task<bool> Add(TaskItem TaskItem)
//        {
//            return choreRepository.Add(TaskItem);
//        }

//        public Task<TaskItem> Update(TaskItem TaskItem)
//        {           
//            return choreRepository.Update(TaskItem);
//        }

//        public void Delete(int choreId)
//        {
//            choreRepository.Delete(choreId);
//        }

//        public void LogicDelete(int choreId)
//        {
//            choreRepository.LogicDelete(choreId);
//        }

//        public Task<IEnumerable<TaskItem>> GetAll(GetRequest<TaskItem>? request)
//        {
//            return choreRepository.GetAll(request);
//        }

//        public Task<IEnumerable<TaskItem>> GetAllLogic(GetRequest<TaskItem>? request)
//        {
//            return choreRepository.GetAllLogic(request);
//        }

//        public TaskItem GetById(int choreId)
//        {  
//            return choreRepository.GetById(choreId).Result;
//        }

//        public ActionResult<int> Count()
//        {
//           return choreRepository.Count();
//        }
//    }
//}
