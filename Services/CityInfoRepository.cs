using CityInfo.API.DBContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository(CityInfoContext context) : ICityInfoRepository
    {
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<(IEnumerable<City>, PaginationMatadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {            
            var cities = context.Cities as IQueryable<City>; 
            
            if(!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                cities = cities.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                cities = cities.Where(c => c.Name.Contains(searchQuery) || (c.Description != null && c.Description.Contains(searchQuery)));
            }

            var totalItemCount = await cities.CountAsync();

            var paginationMetadata = new PaginationMatadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await cities.OrderBy(c => c.Name).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await context.Cities.Include(c => c.PointsOfInterest).FirstOrDefaultAsync(c => c.Id == cityId);
            }
            return await context.Cities.FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await context.PointsOfInterest.FirstOrDefaultAsync(p => p.CityId == cityId && p.Id == pointOfInterestId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await Task.FromResult(context.PointsOfInterest.Where(p => p.CityId == cityId).AsEnumerable());
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            city?.PointsOfInterest.Add(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            context.PointsOfInterest.Remove(pointOfInterest);
        }

        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            return await context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }
    }
}
