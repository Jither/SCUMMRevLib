using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SCUMMRevLib.Decoders;

namespace SCUMMRevLib.Viewers
{
    public sealed class ViewerManager
    {
        public static ViewerManager Current { get { return instance; } }
        private readonly static ViewerManager instance = new ViewerManager();

        private readonly ViewerMap viewers;

        public IList<IViewer> GetViewers(DecoderFormat format)
        {
            return viewers[format];
        }

        private ViewerManager()
        {
            viewers = new ViewerMap();
            RegisterViewers(Assembly.GetExecutingAssembly());
        }

        private void RegisterViewer(DecoderFormat format, IViewer viewer)
        {
            this.viewers.Add(format, viewer);
        }

        public void RegisterViewers(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (typeof(IViewer).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    IViewer viewer = Activator.CreateInstance(type) as IViewer;
                    if (viewer != null)
                    {
                        RegisterViewer(viewer.DecoderFormat, viewer);
                    }
                }
            }
        }
    }
}
