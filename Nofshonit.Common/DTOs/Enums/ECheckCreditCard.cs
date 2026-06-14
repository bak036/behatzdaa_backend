using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Enums
{
    public enum ECheckCreditCard
    {

        WithoutClubCreditCard = 0,
        WithClubCreditCard = 1,
        ExcludedGroupWithoutClubCreditCard = 2,
        ExcludedPersonalWithoutClubCreditCard = 3,
        CusomerServiceWithoutClubCreditCard = 4,
        SkipCheckCreditCard = 5,
        AllProductsAtRegularPrice = 6

    }
}
