using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Engine;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";
        public SuperAdventure()
        {

            InitializeComponent();

            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                _player = Player.CreateDefaultPlayer();
            }

            lblHitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "Level");

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;

            dgvInventory.DataSource = _player.Inventory;

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });

            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;

            dgvQuests.DataSource = _player.Quests;

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name"
            });

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "IsCompleted"
            });

            cboWeapons.DataSource = _player.Weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "Id";

            if (_player.CurrentWeapon != null)
            {
                cboWeapons.SelectedItem = _player.CurrentWeapon;
            }

            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboPotions.DataSource = _player.Potions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "Id";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _player.OnMessage += DisplayMessage;

            _player.MoveTo(_player.CurrentLocation);
            ShowLocationPicture();

        }

        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbMessages.Text += messageEventArgs.Message + Environment.NewLine;

            if (messageEventArgs.AddExtraNewLine)
            {
                rtbMessages.Text += Environment.NewLine;
            }

            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Weapons")
            {
                cboWeapons.DataSource = _player.Weapons;
                if (!_player.Weapons.Any())
                {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }
            if (propertyChangedEventArgs.PropertyName == "Potions")
            {
                cboPotions.DataSource = _player.Potions;
                if (!_player.Potions.Any())
                {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }

            if (propertyChangedEventArgs.PropertyName == "CurrentLocation")
            {
                // Show/hide available movement buttons
                btnNorth.Visible = (_player.CurrentLocation.LocationToNorth != null);
                btnEast.Visible = (_player.CurrentLocation.LocationToEast != null);
                btnSouth.Visible = (_player.CurrentLocation.LocationToSouth != null);
                btnWest.Visible = (_player.CurrentLocation.LocationToWest != null);

                // Display current location name and description
                rtbLocation.Text = _player.CurrentLocation.Name + Environment.NewLine;
                rtbLocation.Text += _player.CurrentLocation.Description + Environment.NewLine;

                if (_player.CurrentLocation.MonsterLivingHere == null)
                {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                }
                else
                {
                    cboWeapons.Visible = _player.Weapons.Any();
                    cboPotions.Visible = _player.Potions.Any();
                    btnUseWeapon.Visible = _player.Weapons.Any();
                    btnUsePotion.Visible = _player.Potions.Any();
                }
            }
        }

        private void SuperAdventure_Load(object sender, EventArgs e)
        {

        }

        private void SuperAdventure_Load_1(object sender, EventArgs e)
        {

        }


        private void btnNorth_Click(object sender, EventArgs e)
        {
            _player.MoveNorth();
            ShowLocationPicture();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            _player.MoveEast();
            ShowLocationPicture();
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            _player.MoveSouth();
            ShowLocationPicture();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            
            _player.MoveWest();
            ShowLocationPicture();
        }

        public void ClearPicture()
        {
            picSpiderForest.Visible = false;
            picBridge.Visible = false;
            picFarmHouse.Visible = false;
            picFields.Visible = false;
            picGarden.Visible = false;
            picAlchemyHut.Visible = false;
            picGuardHouse.Visible = false;
            picTownSquare.Visible = false;
            picHome.Visible = false;
        }

        public void ShowLocationPicture()
        {
            ClearPicture();
            int currnetLocationID = _player.CurrentLocation.ID;

            switch (currnetLocationID)
            {
                case World.LOCATION_ID_HOME:
                    picHome.Visible = true;
                    break;
                case World.LOCATION_ID_TOWN_SQUARE:
                    picTownSquare.Visible = true;
                    break;
                case World.LOCATION_ID_GUARD_POST:
                    picGuardHouse.Visible = true;
                    break;
                case World.LOCATION_ID_ALCHEMIST_HUT:
                    picAlchemyHut.Visible = true;
                    break;
                case World.LOCATION_ID_ALCHEMISTS_GARDEN:
                    picGarden.Visible = true;
                    break;
                case World.LOCATION_ID_FARMHOUSE:
                    picFarmHouse.Visible = true;
                    break;
                case World.LOCATION_ID_FARM_FIELD:
                    picFields.Visible = true;
                    break;
                case World.LOCATION_ID_BRIDGE:
                    picBridge.Visible = true;
                    break;
                case World.LOCATION_ID_SPIDER_FIELD:
                    picSpiderForest.Visible = true;
                    break;


                default:
                    break;
            }
        }


        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void rtbLocation_TextChanged(object sender, EventArgs e)
        {

        }



        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;
            
            _player.UsePotion(potion);

        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            // Get the currently selected weapon from the cboWeapons ComboBox
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            _player.UseWeapon(currentWeapon);
        }

        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _player = Player.ResetPlayer(_player);
            File.WriteAllText(PLAYER_DATA_FILE_NAME, " ");

        }
    }
}
