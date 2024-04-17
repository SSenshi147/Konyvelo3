```
sudo apt install backblaze-b2
backblaze-b2 authorize-account

crontab -e
0 2 * * * backblaze-b2 sync /home/marci/prod/dbs b2://marci-ubuntu-bk/coffee_dbs
5 2 * * * backblaze-b2 sync /home/marci/konyvelo/dbs b2://marci-ubuntu-bk/konyvelo_dbs


# location: /etc/systemd/system/konyvelo.service
[Unit]
Description=Konyvelo
[Service]
WorkingDirectory=/home/marci/konyvelo/linux-x64
ExecStart=/home/marci/konyvelo/linux-x64/Konyvelo
Restart=always
RestartSec=10
SyslogIdentifier=Konyvelo
User=marci
Environment=DOTNET_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target


sudo systemctl daemon-reload
sudo systemctl enable konyvelo
sudo chmod 777 /home/marci/konyvelo/linux-x64/Konyvelo
sudo systemctl start konyvelo
sudo ufw allow 8888
```