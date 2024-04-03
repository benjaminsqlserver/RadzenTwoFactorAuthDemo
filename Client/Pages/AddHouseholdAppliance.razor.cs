using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace HouseholdAppliancesApp.Client.Pages
{
    public partial class AddHouseholdAppliance
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public ConDataService ConDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            householdAppliance = new HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance();
        }
        protected bool errorVisible;
        protected HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance householdAppliance;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await ConDataService.CreateHouseholdAppliance(householdAppliance);
                DialogService.Close(householdAppliance);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}