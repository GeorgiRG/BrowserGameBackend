using BrowserGameBackend.Data;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BrowserGameBackend.Services.Game
{
    public interface ISpeciesService
    {
        public Task<Species>? Create(Species species);
        public Task<Species>? GetSpecies(string name);
        public Task<Species[]> GetAll();
        public Task<bool> Delete(string name);
        public Task<Species>? Update(Species species);
    }

    public class SpeciesService: ISpeciesService
    {
        private readonly GameContext _context;

        public SpeciesService(GameContext context)
        {
            _context = context;
        }

        public async Task<Species>? Create(Species speciesData)
        {     
           
            await _context.Species.AddAsync(speciesData);
            if(await _context.SaveChangesAsync() > 0) return speciesData;
            return null!;
        }

        public async Task<Species>? GetSpecies(string name)
        {
            Species? species = await _context.Species.Where(species => species.Name == name)
                                                    .FirstOrDefaultAsync();
            return species!;
        }

        public async Task<Species[]> GetAll()
        {
            return await _context.Species.ToArrayAsync();
        }
        
        public async Task<bool> Delete(string name)
        {
           return await _context.Species.Where(species => species.Name == name)
                                        .ExecuteDeleteAsync() > 0;
        }

        public async Task<Species>? Update(Species speciesData)
        {
            Species oldData = await _context.Species.Where(species => species.Name == speciesData.Name)
                                                    .FirstAsync();
            oldData.Name = speciesData.Name ?? oldData.Name;
            oldData.Description = speciesData.Description ?? oldData.Description;
            oldData.Color = speciesData.Color ?? oldData.Color;
            oldData.ReproductionSpeed = speciesData.ReproductionSpeed ?? oldData.ReproductionSpeed;
            oldData.ResearchSpeed = speciesData.ResearchSpeed ?? oldData.ResearchSpeed;
            oldData.ProductionSpeed = speciesData.ProductionSpeed ?? oldData.ProductionSpeed;
            oldData.CombatStrength = speciesData.CombatStrength ?? oldData.CombatStrength;
            oldData.SuitComplexity = speciesData.SuitComplexity ?? oldData.SuitComplexity;
            oldData.TemperatureRange = speciesData.TemperatureRange ?? oldData.TemperatureRange;
            oldData.TemperatureAverage = speciesData.TemperatureAverage ?? oldData.TemperatureAverage;
            oldData.Loyalty = speciesData.Loyalty ?? oldData.Loyalty;
            oldData.Name = speciesData.Name ?? oldData.Name;
            oldData.Name = speciesData.Name ?? oldData.Name;

            if (await _context.SaveChangesAsync() > 0) return oldData;
            else return null!;
        }
    }
}
