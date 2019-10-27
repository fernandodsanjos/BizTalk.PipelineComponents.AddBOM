using System;
using System.Collections;
using System.Linq;
using BizTalkComponents.Utils;

namespace BizTalk.PipelineComponents.AddBOM
{
    public partial class AddBOM
    {
        public string Name { get { return "Add BOM"; } }
        public string Version { get { return "1.0"; } }
        public string Description { get { return "Adds BOM To Unicode Stream"; } }

        public void GetClassID(out Guid classID)
        {
            classID = new Guid("f4a779b9-dfea-41a9-9327-f2d079d3abed");
        }

        public void InitNew()
        {

        }

        public IEnumerator Validate(object projectSystem)
        {
            return ValidationHelper.Validate(this, false).ToArray().GetEnumerator();
        }

        public bool Validate(out string errorMessage)
        {
            var errors = ValidationHelper.Validate(this, true).ToArray();

            if (errors.Any())
            {
                errorMessage = string.Join(",", errors);

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        public IntPtr Icon { get { return IntPtr.Zero; } }
    }
}
