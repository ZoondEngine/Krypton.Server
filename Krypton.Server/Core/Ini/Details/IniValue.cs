using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Ini.Details
{
	public struct IniValue
	{
		private static bool TryParseInt(string text, out int value)
		{
			int res;
			if (Int32.TryParse(text,
					System.Globalization.NumberStyles.Integer,
					System.Globalization.CultureInfo.InvariantCulture,
					out res))
			{
				value = res;
				return true;
			}
			value = 0;
			return false;
		}

		private static bool TryParseDouble(string text, out double value)
		{
			double res;
			if (double.TryParse(text,
					System.Globalization.NumberStyles.Float,
					System.Globalization.CultureInfo.InvariantCulture,
					out res))
			{
				value = res;
				return true;
			}
			value = Double.NaN;
			return false;
		}

		public string Value;

		public IniValue(object value)
		{
			var formattable = value as IFormattable;
			if (formattable != null)
			{
				Value = formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
			}
			else
			{
				Value = value != null ? value.ToString() : null;
			}
		}

		public IniValue(string value)
		{
			Value = value;
		}

		public bool ToBool(bool valueIfInvalid = false)
		{
			bool res;
			if (TryConvertBool(out res))
			{
				return res;
			}
			return valueIfInvalid;
		}

		public bool TryConvertBool(out bool result)
		{
			if (Value == null)
			{
				result = default(bool);
				return false;
			}
			var boolStr = Value.Trim().ToLowerInvariant();
			if (boolStr == "true")
			{
				result = true;
				return true;
			}
			else if (boolStr == "false")
			{
				result = false;
				return true;
			}
			result = default(bool);
			return false;
		}

		public int ToInt(int valueIfInvalid = 0)
		{
			int res;
			if (TryConvertInt(out res))
			{
				return res;
			}
			return valueIfInvalid;
		}

		public bool TryConvertInt(out int result)
		{
			if (Value == null)
			{
				result = default(int);
				return false;
			}
			if (TryParseInt(Value.Trim(), out result))
			{
				return true;
			}
			return false;
		}

		public double ToDouble(double valueIfInvalid = 0)
		{
			double res;
			if (TryConvertDouble(out res))
			{
				return res;
			}
			return valueIfInvalid;
		}

		public bool TryConvertDouble(out double result)
		{
			if (Value == null)
			{
				result = default(double);
				return false;
			}
			if (TryParseDouble(Value.Trim(), out result))
			{
				return true;
			}
			return false;
		}

		public string GetString()
		{
			return GetString(true, false);
		}

		public string GetString(bool preserveWhitespace)
		{
			return GetString(true, preserveWhitespace);
		}

		public string GetString(bool allowOuterQuotes, bool preserveWhitespace)
		{
			if (Value == null)
			{
				return "";
			}
			var trimmed = Value.Trim();
			if (allowOuterQuotes && trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[trimmed.Length - 1] == '"')
			{
				var inner = trimmed.Substring(1, trimmed.Length - 2);
				return preserveWhitespace ? inner : inner.Trim();
			}
			else
			{
				return preserveWhitespace ? Value : Value.Trim();
			}
		}

		public T Get<T>()
		{
			if (typeof(T) == typeof(bool))
				return (T)(object)ToBool();

			if (typeof(T) == typeof(double))
				return (T)(object)ToDouble();

			if (typeof(T) == typeof(int))
				return (T)(object)ToInt();

			if (typeof(T) == typeof(string))
				return (T)(object)GetString();

			return default;
		}

		public override string ToString()
		{
			return Value;
		}

		public static implicit operator IniValue(byte o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(short o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(int o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(sbyte o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(ushort o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(uint o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(float o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(double o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(bool o)
		{
			return new IniValue(o);
		}

		public static implicit operator IniValue(string o)
		{
			return new IniValue(o);
		}

		public static IniValue Default { get; } = new IniValue();
	}
}
