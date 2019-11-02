using InfructructureDataInterfaces.Models;
using InfructructureDataInterfaces.Repositories;
using InfrustructureData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using InfrustructureData.Mappers;
using InfrustructureData.DataModels;
using Microsoft.EntityFrameworkCore;

namespace InfrustructureData.Repositories
{
    public class SpeachRepository : IRepositories<RepoSpeach>
    {
        private AutoContext db;

        public SpeachRepository(AutoContext db)
        {
            this.db = db;
        }
        public void Create(RepoSpeach item)
        {
            db.Speaches.Add(item.FromRepoSpeachToSpeach());
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Speach sp = db.Speaches.Find(id);
            db.Speaches.Remove(sp);
            db.SaveChanges();
        }

        public RepoSpeach Get(int id)
        {
            return db.Speaches.Find(id).FromSpeachToRepoSpeach();
        }

        public IEnumerable<RepoSpeach> GetAll()
        {
            return db.Speaches.Select(x => x.FromSpeachToRepoSpeach()).ToList();
        }

        public IEnumerable<RepoSpeach> GetRange(int id)
        {
            return db.Speaches.Where(x => x.IdCar == id).Select(x => x.FromSpeachToRepoSpeach()).ToList();
        }
               
                               
        public void Update(RepoSpeach item)
        {
            Speach sp = db.Speaches.Find(item.Id);
            sp.Name = item.Name;
            sp.LastMes = item.LastMes;
            sp.IdUser = item.IdUser;
            sp.IdOwner = item.IdOwner;
            sp.IdCar = item.IdCar;
            sp.IsDeleted = item.IsDeleted;
            db.Entry(sp).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteWasteEntities()
        {
            List<Speach> speaches = db.Speaches.Where(x => x.IsDeleted == true).ToList();
            for (int i = 0; i < speaches.Count; i++)
            {
                List<Message> messages = db.Messages.Where(x => x.SpeachId == speaches[i].Id).ToList();
                if (messages != null)
                    db.Messages.RemoveRange(messages);
            }
            db.Speaches.RemoveRange(speaches);
            db.SaveChanges();
        }

        public void DeleteRange(List<RepoSpeach> items)
        {            
            foreach (RepoSpeach b in items)
            {
                Speach delete = db.Speaches.FirstOrDefault(x => x.Id == b.Id);
                db.Speaches.Remove(delete);
            }
            db.SaveChanges();
        }
    }
}
