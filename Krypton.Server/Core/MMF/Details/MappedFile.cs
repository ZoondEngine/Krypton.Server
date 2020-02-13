using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading.Tasks;

namespace Krypton.Server.Core.MMF.Details
{
	public class MappedFile
	{
		private MemoryMappedFile m_file { get; set; }
		private string m_file_path { get; set; }

		public MappedFile()
		{ }

		public bool Create(string path)
		{
			if(File.Exists(path))
			{
				m_file_path = path;
				m_file = MemoryMappedFile.CreateFromFile(path);
				return true;
			}

			throw new FileNotFoundException($"File: '{path}' not found");
		}

		public MemoryMappedViewAccessor CreateAccessor()
			=> m_file.CreateViewAccessor();

		public MemoryMappedViewStream CreateStream()
			=> m_file.CreateViewStream();

		public MemoryMappedFile GetMappedFile()
			=> m_file;

		public MemoryStream GetMemoryStream()
			=> new MemoryStream(FullMMFToBytes());

		public async Task<MemoryStream> GetMemoryStreamAsync()
			=> new MemoryStream(await FullMMFToBytesAsync());

		public string GetComparableString()
			=> m_file_path;

		private byte[] ToBytes(int offset, int size)
		{
			using(var stream = CreateStream())
			{
				if((int)stream.Length >= size)
				{
					var buffer = new byte[size];
					if(stream.CanRead)
					{
						stream.Read(buffer, offset, size);
						return buffer;
					}
				}
			}

			return null;
		}
		private async Task<byte[]> ToBytesAsync(int offset, int size)
		{
			using (var stream = CreateStream())
			{
				if ((int)stream.Length >= size)
				{
					var buffer = new byte[size];
					if (stream.CanRead)
					{
						await stream.ReadAsync(buffer, offset, size);
						return buffer;
					}
				}
			}

			return null;
		}

		private byte[] FullMMFToBytes()
		{
			using (var stream = CreateStream())
				return ToBytes(0, (int)stream.Length);
		}
		private async Task<byte[]> FullMMFToBytesAsync()
		{
			using (var stream = CreateStream())
				return await ToBytesAsync(0, (int)stream.Length);
		}

		public static bool operator ==(MappedFile first, MappedFile second)
			=> first.GetComparableString() == second.GetComparableString();

		public static bool operator !=(MappedFile first, MappedFile second)
			=> first.GetComparableString() != second.GetComparableString();
	}
}
