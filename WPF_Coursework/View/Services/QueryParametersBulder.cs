using System.Text;

namespace View.Services {
	public class QueryParametersBulder {
		private StringBuilder _stringBuilder = new StringBuilder();
		public int ParamethersCount { get; private set; } = 0;

		public void AddParameter(string name, object value) {
			if (value is null)
				return;

			if (_stringBuilder.Length != 0)
				_stringBuilder.Append("&");

			_stringBuilder.Append($"{name}={value}");
			ParamethersCount++;
		}

		public override string ToString() {
			if (ParamethersCount == 0)
				return string.Empty;

			return $"?{_stringBuilder}";
		}
	}
}
