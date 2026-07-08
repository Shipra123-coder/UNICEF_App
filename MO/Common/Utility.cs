using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Common
{
    public enum EnumAlert
    {
        Success = 1,
        Information,
        Warning,
        Error
    }

    public enum EnumProfileQuestion
    {
        [Display(Name = "विज्ञापन, विक्रय, प्रचार और प्रसार से संबंधित गतिविधि")]
        AdvertisingSalesPromotion = 1,

        [Display(Name = "पंच-गौरव के अंतर्गत सहायतार्थ अनुदान (गैर संवेतन) से संबंधित गतिविधि का नाम")]
        GrantRelatedActivity = 2,

        [Display(Name = "पंच-गौरव के अंतर्गत उत्सव और प्रदर्शनियों से संबंधित गतिविधि")]
        EventsExhibitions = 3,

        [Display(Name = "पंच-गौरव के अंतर्गत कम्प्यूटरीकरण एवं संबंधित संचार से संबधिंत गतिविधि")]
        ComputerizationCommunication = 4,

        [Display(Name = "पंच-गौरव के अंतर्गत वृहद् निर्माण कार्य से संबधिंत गतिविधि")]
        ConstructionWork = 5,

        [Display(Name = "पंच-गौरव के अंतर्गत अन्य गतिविधि")]
        OtherActivity = 6,
        [Display(Name = "मशीनरी और साज सामान/औजार एवं संयंत्र")]
        MachineryEquipment = 7,
        [Display(Name = "प्रशिक्षण, भ्रमण एवं सम्मेलन व्यय")]
        TrainingConferenceExpenses = 8,

    }


}
