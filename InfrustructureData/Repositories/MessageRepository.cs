using InfructructureDataInterfaces.Models;
using InfructructureDataInterfaces.Repositories;
using InfrustructureData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrustructureData.Mappers;
using InfrustructureData.DataModels;

namespace InfrustructureData.Repositories
{
    public class MessageRepository : IRepositories<RepoMessage>
    {
        private AutoContext db;

        public MessageRepository(AutoContext db)
        {
            this.db = db;
        }
        public void Create(RepoMessage item)
        {
            db.Messages.Add(item.FromRepoMessageToMessage());
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Message mes = db.Messages.Find(id);
            db.Messages.Remove(mes);
            db.SaveChanges();
        }

        public RepoMessage Get(int id)
        {
            return db.Messages.Find(id).FromMessageToRepoMessage();
        }

        public IEnumerable<RepoMessage> GetAll()
        {
            return db.Messages.Select(x => x.FromMessageToRepoMessage()).ToList();
        }

        public IEnumerable<RepoMessage> GetRange(int id)
        {
            return db.Messages.Where(x=>x.SpeachId==id).Select(x => x.FromMessageToRepoMessage()).ToList();
        }

        public void Update(RepoMessage item)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(List<RepoMessage> items)
        {            
            foreach (RepoMessage b in items)
            {
                if (b.Id != 0)
                {
                    Message delete = db.Messages.FirstOrDefault(x => x.Id == b.Id);
                    db.Messages.Remove(delete);
                }
            }
            db.SaveChanges();
        }

        public void DeleteWasteEntities()
        {
            throw new NotImplementedException();
        }
    }
}
