using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HouseholdAppliancesApp.Server.Controllers.ConData
{
    [Route("odata/ConData/HouseholdAppliances")]
    public partial class HouseholdAppliancesController : ODataController
    {
        private HouseholdAppliancesApp.Server.Data.ConDataContext context;

        public HouseholdAppliancesController(HouseholdAppliancesApp.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> GetHouseholdAppliances()
        {
            var items = this.context.HouseholdAppliances.AsQueryable<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>();
            this.OnHouseholdAppliancesRead(ref items);

            return items;
        }

        partial void OnHouseholdAppliancesRead(ref IQueryable<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> items);

        partial void OnHouseholdApplianceGet(ref SingleResult<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/HouseholdAppliances(ApplianceID={ApplianceID})")]
        public SingleResult<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> GetHouseholdAppliance(int key)
        {
            var items = this.context.HouseholdAppliances.Where(i => i.ApplianceID == key);
            var result = SingleResult.Create(items);

            OnHouseholdApplianceGet(ref result);

            return result;
        }
        partial void OnHouseholdApplianceDeleted(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);
        partial void OnAfterHouseholdApplianceDeleted(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);

        [HttpDelete("/odata/ConData/HouseholdAppliances(ApplianceID={ApplianceID})")]
        public IActionResult DeleteHouseholdAppliance(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.HouseholdAppliances
                    .Where(i => i.ApplianceID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnHouseholdApplianceDeleted(item);
                this.context.HouseholdAppliances.Remove(item);
                this.context.SaveChanges();
                this.OnAfterHouseholdApplianceDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnHouseholdApplianceUpdated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);
        partial void OnAfterHouseholdApplianceUpdated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);

        [HttpPut("/odata/ConData/HouseholdAppliances(ApplianceID={ApplianceID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutHouseholdAppliance(int key, [FromBody]HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.HouseholdAppliances
                    .Where(i => i.ApplianceID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnHouseholdApplianceUpdated(item);
                this.context.HouseholdAppliances.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.HouseholdAppliances.Where(i => i.ApplianceID == key);
                
                this.OnAfterHouseholdApplianceUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/HouseholdAppliances(ApplianceID={ApplianceID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchHouseholdAppliance(int key, [FromBody]Delta<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.HouseholdAppliances
                    .Where(i => i.ApplianceID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnHouseholdApplianceUpdated(item);
                this.context.HouseholdAppliances.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.HouseholdAppliances.Where(i => i.ApplianceID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnHouseholdApplianceCreated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);
        partial void OnAfterHouseholdApplianceCreated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnHouseholdApplianceCreated(item);
                this.context.HouseholdAppliances.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.HouseholdAppliances.Where(i => i.ApplianceID == item.ApplianceID);

                

                this.OnAfterHouseholdApplianceCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
