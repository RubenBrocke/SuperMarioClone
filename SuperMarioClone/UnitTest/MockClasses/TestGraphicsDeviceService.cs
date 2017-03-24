using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace UnitTest.MockClasses
{
    public class TestGraphicsDeviceService : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
    {
        GraphicsDevice _GraphicsDevice;
        Form HiddenForm;

        public TestGraphicsDeviceService()
        {
            HiddenForm = new Form()
            {
                Visible = false,
                ShowInTaskbar = false
            };
            var Parameters = new PresentationParameters()
            {
                BackBufferWidth = 1280,
                BackBufferHeight = 720,
                DeviceWindowHandle = HiddenForm.Handle,
                PresentationInterval = PresentInterval.Immediate,
                IsFullScreen = false
            };
            _GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach, Parameters);
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _GraphicsDevice; }
        }

        public IGraphicsDeviceService GraphicsDeviceService
        {
            get { return this; }
        }

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        public bool BeginDraw()
        {
            return true;
        }

        public void CreateDevice()
        {
            
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public void EndDraw()
        {
            
        }

        public void Release()
        {
            _GraphicsDevice.Dispose();
            _GraphicsDevice = null;

            HiddenForm.Close();
            HiddenForm.Dispose();
            HiddenForm = null;
        }
    }
}