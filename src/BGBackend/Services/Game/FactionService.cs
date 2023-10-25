using BrowserGameBackend.Data;
using BrowserGameBackend.Enums;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services.Utilities;
using BrowserGameBackend.Types.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace BrowserGameBackend.Services.Game
{
    public interface IFactionService
    {
        public Task<Faction>? Create(Faction faction);
        public Task<Faction>? GetFaction(string name);
        public Task<Faction[]> GetAll();
        public Task<bool> Delete(string name);
        public Task<Faction>? Update(Faction faction);
    }

    public class FactionService: IFactionService
    {
        private readonly GameContext _context;
        private readonly Factions _factions = new();

        public FactionService(GameContext context)
        {
            _context = context;
        }

        public async Task<Faction>? Create(Faction factionData)
        {     
            if (!_factions.IsValidValue(factionData.Name)) return null!;
            int capitalId = await _context.StarSystems.Where(star => star.Faction == factionData.Name && star.Capital)
                                                    .Select(star => star.Id)
                                                    .FirstAsync();
            factionData.CapitalSystemId = capitalId;
            Bot? rulerBot = await _context.Bots.Where(bot => bot.Faction == factionData.Name)
                                            .OrderByDescending(bot => bot.PowerLevel)
                                            .FirstAsync();
            factionData.RulerId = rulerBot.Id;
            factionData.Ruler = rulerBot;
            await _context.Factions.AddAsync(factionData);
            if(await _context.SaveChangesAsync() > 0) return factionData;
            return null!;
        }

        public async Task<Faction>? GetFaction(string name)
        {
            Faction? faction = await _context.Factions.Where(faction => faction.Name == name)
                                                    .FirstOrDefaultAsync();
            return faction!;
        }

        public async Task<Faction[]> GetAll()
        {
            return await _context.Factions.ToArrayAsync();
        }
        
        public async Task<bool> Delete(string name)
        {
           return await _context.Factions.Where(faction => faction.Name == name)
                                        .ExecuteDeleteAsync() > 0;
        }

        //for modifiers 
        public async Task<Faction>? Update(Faction factionData)
        {
            Faction oldData = await _context.Factions.Where(faction => faction.Name == factionData.Name)
                                        .FirstAsync();
            oldData.Name = factionData.Name ?? oldData.Name;
            oldData.Description = factionData.Description ?? oldData.Description;
            oldData.Color = factionData.Color ?? oldData.Color;
            oldData.SomeModifier = factionData.SomeModifier ?? oldData.SomeModifier;

            if (await _context.SaveChangesAsync() > 0) return oldData;
            else return null!;
        }
    }
}