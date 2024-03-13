using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LocalData.Entities {
	public class Setting {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
