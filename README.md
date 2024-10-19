# Home Power Manager

## My Problem
I have a house equipped with solar panels, house batteries, an EV charger, and an immersion heater. 
The house batteries are charged by the solar panels during daylight hours and by the grid during the cheap night tariff. 
However, if both the EV and the house batteries are charging simultaneously, the EV charger detects a sudden drop in power when the immersion heater turns on, causing it to stop charging and requiring manual intervention. 
This typically happens at night or early morning when I am not available to monitor it.

The immersion heater is controlled by a smart WiFi-enabled timer, which I can adjust to turn on at different times. 
The GivEnergy inverter's charging schedule is limited to a simple start/stop configuration and does not support multiple schedules. 
When the house batteries are not charging, the immersion heater starts to drain the house battery.

As a short-term solution, I ensure that the house battery stops charging before the immersion heater turns on. 
However, this can lead to the house battery being drained on a potentially cloudy day, leaving insufficient battery power to cover the peak kWh unit cost.

## My Solution
The house battery will always charge at night, but I can adjust its start and end times if the EV is charging.
If the EV is not charging, I can set the house battery to stop charging when the peak unit pricing begins.

If the EV is charging, I can configure the house battery to stop charging a minute or two before the immersion heater turns on.
If the immersion heater is scheduled to turn off before peak unit pricing starts, the house battery will be scheduled to charge during that time.

When peak unit pricing begins, I can then set the house battery charging schedule to a default based on the next scheduled night-time operation of the immersion heater.


## API References
### MyEnergi API
https://myenergi.info/api-f54/
https://github.com/twonk/MyEnergi-App-Api
https://github.com/Mausy5043/lektrix/blob/master/docs/ZAPPI.md
https://github.com/twonk/MyEnergi-App-Api/blob/master/README.md