#!/bin/bash

# Check if feh is installed
command -v feh >/dev/null 2>&1 || { echo >&2 "Please install 'feh' to run this script."; exit 1; }

# Check if a web link parameter is provided
if [ $# -eq 0 ]; then
    echo "Please provide a web link to a picture as a parameter."
    exit 1
fi

# Save the web link parameter
picture_link=$1

# Check if a previous picture is displayed
if pgrep -x feh >/dev/null; then
    # Kill the previous feh process
    pkill -x feh

    # Wait for the process to exit
    sleep 1
fi

# Download the new picture using wget
temp_file="/tmp/fullscreen_picture.jpg"
wget -q -O "$temp_file" "$picture_link"

# Display the new picture in fullscreen with a transition effect using feh
feh --auto-zoom --fullscreen --quiet --hide-pointer --image-bg black --zoom fill --no-menus --slideshow-delay 5 --no-fehbg "$temp_file"

# Delete the temporary file
rm "$temp_file"