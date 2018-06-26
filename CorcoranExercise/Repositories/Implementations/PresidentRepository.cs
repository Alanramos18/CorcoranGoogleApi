using CorcoranExercise.Models;
using CorcoranExercise.Models.Entities;
using CorcoranExercise.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CorcoranExercise.Utilities;
using CorcoranExercise.Models.ViewModel;

namespace CorcoranExercise.Repositories.Implementations
{
    public class PresidentRepository : IPresidentRepository
    {
        private PresidentServiceContext context;

        public PresidentRepository()
        {
            context = new PresidentServiceContext();
        }

        public List<President> GetAll()
        {
            try
            {
                List<President> list = new List<President>();

                foreach (var value in context.result.Values)
                {
                    //Creates the president object
                    var president = new President()
                    {
                        Name = value[0].ToString(),
                        Birthday = Convert.ToDateTime(value[1]),
                        Birthplace = value[2].ToString(),
                    };

                    //Checks if deathday and deathplace is null
                    if (value.Count > 3)
                    {
                        president.Deathday = Convert.ToDateTime(value[3]);
                        president.Deathplace = value[4].ToString();
                    }
                    else
                    {
                        president.Deathday = (DateTime?)null;
                        president.Deathplace = "";
                    }

                    list.Add(president);
                }

                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<President> Get(string name)
        {
            try
            {
                var list = GetAll();

                return list.Where(x => x.Name.Contains(name)).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<President> ChangeOrder(SortVM viewModel)
        {
            try
            {
                var list = GetAll();

                switch (viewModel.Name)
                {
                    case ColumnName.BirthDay:
                        if (viewModel.IsAscending)
                            return list.OrderBy(x => !x.Deathday.HasValue)
                                .ThenBy(x => x.Birthday)
                                .ToList();
                        else
                            return list.OrderByDescending(x => x.Deathday.HasValue)
                                .ThenByDescending(x => x.Birthday)
                                .ToList();

                    case ColumnName.DeathDay:
                        if (viewModel.IsAscending)
                            return list.OrderBy(x => !x.Deathday.HasValue)
                                .ToList();
                        else
                            return list.OrderByDescending(x => x.Deathday.HasValue)
                                .ThenByDescending(x => x.Deathday)
                                .ToList();
                    default:
                        throw new Exception("Wrong Column Name");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}