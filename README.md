# Brute Force Project
This project demonstrates a brute force attack to discover a designated password using C#. The program attempts to authenticate with a server by trying all possible password combinations within a specified range.

## Description
The program is designed to:
1. Connect to a given IP address and port.
2. Use multiple threads to distribute the password guessing workload.
3. Attempt to authenticate using a username and various password combinations.
4. Identify and print the correct password once it is found.

## How It Works
Setup: Define the IP address, port, username, and the total number of possible passwords.
Multithreading: Utilize multiple threads to speed up the brute force attack.
Password Guessing: Each thread generates a range of passwords to try.
Server Response: The program sends HTTP POST requests to the server, checking each password.
Password Discovery: If the server responds positively, the correct password is printed, and the program terminates.
