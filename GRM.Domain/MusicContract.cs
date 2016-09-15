using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Domain
{
    public class MusicContract
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public List<DistributionPartner> Usages { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool HasADeliveryPartnerNamed(string deliveryPartnerName)
        {
            return Usages.Any(usage => usage.Partner.Trim().ToLower() == deliveryPartnerName.Trim().ToLower());
        }

        public bool IsCurrentAsOf(DateTime effectiveDate)
        {
            var endDateExists = (EndDate.Ticks != 0);
            var contractHasNotYetStarted = (StartDate.Ticks > effectiveDate.Ticks);
            var contractHasExpired = (EndDate.Ticks < effectiveDate.Ticks);


            if (contractHasNotYetStarted)
            {
                return false;
            }
            
            if (endDateExists && contractHasExpired)
            {
                return false;
            }            

            return true;
        }
        
    }
}
