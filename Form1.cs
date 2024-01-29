using System; 
using System.Collections.Generic; 
using System.ComponentModel; 
using System.Data; 
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Media; // for SoundPlayer
using System.Windows.Forms;

namespace MysticMask
{
    public partial class MainForm : Form
    {
        private SoundPlayer soundPlayer; // for playing sound
        private bool isMusicPlaying = true; // for checking if music is playing


        public MainForm()
        {
            InitializeComponent();
            InitializeMusic(); // initialize music
        }

        private void InitializeMusic()
        {
            // Load the music file
            soundPlayer = new SoundPlayer("moose.wav");

            // Play the music
            PlayMusic();
        }

        private void PlayMusic()
        {
            if (isMusicPlaying)
            {
                soundPlayer.PlayLooping();
            }
            else
            {
                soundPlayer.Stop();
            }
        }

        private void MusicIcon_Click(object sender, EventArgs e)
        {
            // Toggle music on/off
            isMusicPlaying = !isMusicPlaying;

            // Update icon and play/pause music accordingly
            if (isMusicPlaying)
            {
                MusicIcon.Image = Properties.Resources.VolumeOnIcon;
                PlayMusic();
            }
            else
            {
                MusicIcon.Image = Properties.Resources.VolumeOffIcon;
                PlayMusic();
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }
    }
}

