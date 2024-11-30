using System.Collections.Generic;
using System.Threading.Tasks;
using Plpext.Core.Models;

namespace Plpext.UI.Services;

public interface IConvertService
{
    public Task ConvertFilesAsync(IEnumerable<AudioFile> files, string outputFolder);
}