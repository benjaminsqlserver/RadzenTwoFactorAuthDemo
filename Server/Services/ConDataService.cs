using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using HouseholdAppliancesApp.Server.Data;

namespace HouseholdAppliancesApp.Server
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportHouseholdAppliancesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/householdappliances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/householdappliances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportHouseholdAppliancesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/householdappliances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/householdappliances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnHouseholdAppliancesRead(ref IQueryable<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> items);

        public async Task<IQueryable<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>> GetHouseholdAppliances(Query query = null)
        {
            var items = Context.HouseholdAppliances.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnHouseholdAppliancesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHouseholdApplianceGet(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);
        partial void OnGetHouseholdApplianceByApplianceId(ref IQueryable<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> items);


        public async Task<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> GetHouseholdApplianceByApplianceId(int applianceid)
        {
            var items = Context.HouseholdAppliances
                              .AsNoTracking()
                              .Where(i => i.ApplianceID == applianceid);

 
            OnGetHouseholdApplianceByApplianceId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnHouseholdApplianceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnHouseholdApplianceCreated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);
        partial void OnAfterHouseholdApplianceCreated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);

        public async Task<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> CreateHouseholdAppliance(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance householdappliance)
        {
            OnHouseholdApplianceCreated(householdappliance);

            var existingItem = Context.HouseholdAppliances
                              .Where(i => i.ApplianceID == householdappliance.ApplianceID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.HouseholdAppliances.Add(householdappliance);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(householdappliance).State = EntityState.Detached;
                throw;
            }

            OnAfterHouseholdApplianceCreated(householdappliance);

            return householdappliance;
        }

        public async Task<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> CancelHouseholdApplianceChanges(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnHouseholdApplianceUpdated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);
        partial void OnAfterHouseholdApplianceUpdated(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);

        public async Task<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> UpdateHouseholdAppliance(int applianceid, HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance householdappliance)
        {
            OnHouseholdApplianceUpdated(householdappliance);

            var itemToUpdate = Context.HouseholdAppliances
                              .Where(i => i.ApplianceID == householdappliance.ApplianceID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(householdappliance);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterHouseholdApplianceUpdated(householdappliance);

            return householdappliance;
        }

        partial void OnHouseholdApplianceDeleted(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);
        partial void OnAfterHouseholdApplianceDeleted(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance item);

        public async Task<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> DeleteHouseholdAppliance(int applianceid)
        {
            var itemToDelete = Context.HouseholdAppliances
                              .Where(i => i.ApplianceID == applianceid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnHouseholdApplianceDeleted(itemToDelete);


            Context.HouseholdAppliances.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterHouseholdApplianceDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}