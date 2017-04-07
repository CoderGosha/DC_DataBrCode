using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;


namespace DataBarCode
{
    class Notification
    {
    }

    public static class Sound
    {
        private static string soundWarning = "\\sound\\soundWarning.wav";
        public static void PlaySoundWarning()
        {
            OpenNETCF.Media.SystemSounds.Beep.Play();
        }

        public static void PlaySoundWarningVolumeHIGH()
        {
            BeeMobile.Volumes v = BeeMobile.VolumeControl.Volume;
            BeeMobile.VolumeControl.Volume = BeeMobile.Volumes.HIGH;
            OpenNETCF.Media.SystemSounds.Beep.Play();
            BeeMobile.VolumeControl.Volume = v;
        }

        public static void PlaySoundWarningVolumeVeryHIGH()
        {
            BeeMobile.Volumes v = BeeMobile.VolumeControl.Volume;
            BeeMobile.VolumeControl.Volume = BeeMobile.Volumes.VERY_HIGH;
            OpenNETCF.Media.SystemSounds.Beep.Play();
            BeeMobile.VolumeControl.Volume = v;
        }

        public static void PlaySoundExclamationVolumeVeryHIGH()
        {
            BeeMobile.Volumes v = BeeMobile.VolumeControl.Volume;
            BeeMobile.VolumeControl.Volume = BeeMobile.Volumes.VERY_HIGH;
            OpenNETCF.Media.SystemSounds.Exclamation.Play();
            BeeMobile.VolumeControl.Volume = v;
        }
    }

    public static class LedIndicator
    {
        public static void LedRedOn(int mmm)
        {

        }
    }

    public static class Vibration
    {

        public static void PlayVibrationWindows(int mmm)
        {

            OpenNETCF.WindowsCE.Notification.Led vib = new OpenNETCF.WindowsCE.Notification.Led();

            //---start vibration---

            vib.SetLedStatus(1, OpenNETCF.WindowsCE.Notification.Led.LedState.On);

            System.Threading.Thread.Sleep(mmm);

            //---stop vibration---

            vib.SetLedStatus(1, OpenNETCF.WindowsCE.Notification.Led.LedState.Off);
        }

        public static void PlayVibration(int mmm)
        {

            OpenNETCF.WindowsCE.Notification.Led vib = new OpenNETCF.WindowsCE.Notification.Led();

            //---start vibration---

            vib.SetLedStatus(1, OpenNETCF.WindowsCE.Notification.Led.LedState.On);

            System.Threading.Thread.Sleep(mmm);

            //---stop vibration---

            vib.SetLedStatus(1, OpenNETCF.WindowsCE.Notification.Led.LedState.Off);
        }

    }

    public static class ScreenShot
    {
        public static bool MakeShot()
        {
            bool bRet = false;
            try
            {
                string filename = "Log/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".bmp";
                BeeMobile.CaptureScreen.Snapshot(filename, new System.Drawing.Rectangle(0, 0, 480, 640));
                bRet = true;
            }
            catch (Exception ex) { CLog.WriteException("Notification", "MakeShot", ex.ToString()); }
            return bRet;
        }

        public static bool MakeShot(string Module)
        {
            bool bRet = false;
            try
            {
                string filename = "Log/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_" + Module + ".bmp";
                BeeMobile.CaptureScreen.Snapshot(filename, new System.Drawing.Rectangle(0, 0, 480, 640));
                bRet = true;
            }
            catch (Exception ex) { CLog.WriteException("Notification", "MakeShot", ex.ToString()); }
            return bRet;
        }
    }


}
