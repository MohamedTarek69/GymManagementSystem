using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextDataSeeding
    {
        public static bool SeedData(GymDbContext context)
        {
            try
            {
                var HasPlans = context.Plans.Any();
                var HasCategories = context.Categories.Any();

                if (HasPlans && HasCategories) return false;

                if (!HasPlans)
                {
                    var Plans = loadDataFromJson<Plan>("plans.json");
                    if (Plans.Any())
                    {
                        context.Plans.AddRange(Plans);
                    }
                }

                if (!HasCategories)
                {
                    var Categories = loadDataFromJson<Category>("categories.json");
                    if (Categories.Any())
                    {
                        context.Categories.AddRange(Categories);
                    }
                }

                return context.SaveChanges() > 0;
            }
            catch (Exception? EX) 
            {
                Console.WriteLine(EX.Message);
                return false;
            }

        }

        private static List<T> loadDataFromJson<T>(string filePath)
        {
            //D:\Course .NET\07 ASP .Net MVC\MVC Project\GymManagementSystemSolution\GymManagementPL\
            //wwwroot\Files\
            //plans.json
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", filePath);
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            var Data = File.ReadAllText(FilePath);

            var Options = new JsonSerializerOptions() 
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();



        }
    }
}
