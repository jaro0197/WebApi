using System.Runtime.CompilerServices;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        return Ok(city.PointsOfInterest);
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> Get(int cityId,
        int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null)
        {
            return NotFound();
        }

        var pof = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);

        if (pof == null)
        {
            return NotFound();
        }

        return Ok(pof);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId,
        PointOfInterestForCreationDto pointOfInterest)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var maxId = CitiesDataStore.Current.Cities.SelectMany(
            c => c.PointsOfInterest).Max(p => p.Id);

        var finanResult = new PointOfInterestDto()
        {
            Id = ++maxId, Name = pointOfInterest.Name, Description = pointOfInterest.Description
        };

        city.PointsOfInterest.Add(finanResult);

        return CreatedAtRoute("GetPointOfInterest", new
        {
            cityId = cityId, pointOfInterestId = finanResult.Id
        }, finanResult);
    }

}