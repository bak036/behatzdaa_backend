using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
	public interface IHistadrutService
	{
		Task<JObject> GetHistadrutUser(UserDTO user);
	}
}
