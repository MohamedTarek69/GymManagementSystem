﻿using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repository.Classes
{
    internal class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext _dbContext;

        //public GymDbContext DbContext { get; set; } = new GymDbContext();
        //private readonly GymDbContext _dbContext = new GymDbContext();

        public MemberRepository(GymDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var member = _dbContext.Members.Find(id);

            if (member == null) return 0;

            _dbContext.Members.Remove(member);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Member> GetAll() => _dbContext.Members.ToList();

        public Member? GetById(int id) => _dbContext.Members.Find(id);

        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
    }
}
