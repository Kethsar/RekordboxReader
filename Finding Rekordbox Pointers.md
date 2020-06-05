* Grab [CheatEngine](https://www.cheatengine.org/downloads.php) if you don't already have it.
* In CheatEngine, load the Rekordbox process.

![CheatEngine 1](img/ce-1.png)

## Artist/Title Pointers
* In Rekordbox, load a song into the deck you want to find pointers for.
* On the right half of CheatEngine, set the Value Type to String, and enter the exact song title or artist name loaded into the deck.

![CheatEngine 2](img/ce-2.png)

* Set the Value Type to Array of byte, add "00" at the end, and click First Scan. Adding "00" to the end of the array filters a lot of false-positive addresses that might have the file name loaded instead.

![CheatEngine 3](img/ce-3.png)

* Add all addresses found to the list at the bottom by double-clicking them.

![CheatEngine 4](img/ce-4.png)

* For each address, right-click and choose "Browser this memory region" and make sure the value is surrounded by mostly garbage data. The first image below is similar to what you want to see. If it looks like the second image below (surrounded by other similar values or file names), it might find a valid pointer but not of the type we are looking for. Remove any bad addresses from the list.

![CheatEngine 5](img/ce-5.png)
![CheatEngine 6](img/ce-6.png)

### EZ MODO
1. Choose one address, right-click and select "Pointer scan for this address" and click OK in the window that pops-up. You will have to select a location to save a file it generates, this can be chosen anywhere. ![CheatEngine 7](img/ce-7.png)
2. Load a new track into the deck. In the pointer scan window, right-click anywhere in the value list and click Resync modulelist. ![CheatEngine 8](img/ce-8.png)
3. Search for the new string you want in the list. You can either confirm if multiple instances point to the same address or not.
4. In the pointer scan window, click "Pointer scanner" at the top and select "Rescan memory." In the window that pops up, enter one of the addresses that has the value you want, and click OK, saving the file when prompted. ![CheatEngine 9](img/ce-9.png)
5. You should now have a much shorter list of addresses. Repeat step 2. There is a good chance all the pointers will point to the same new address. You can repeat step 2 as many times as you want to be safe.
6. Once you are comfortable with the list of pointers knowing they properly point to the value you want, you have two options.
  1. The first option is to choose one of the pointers and use it. This pointer is only guaranteed to work with the deck mode you had Rekordbox running in when finding the pointer.
  2. The second option is to change rekordbox's deck mode, Resync the modulelist in the pointer scan window, and repeat step 2 as needed. You will also need to change the deck mode back even if some of the pointers seem to be fine, as they might not actually be fine. This has a high chance of failing in general, and going Hard Mode might be needed to increase your chances of finding a pointer that works regardless of the deck mode being changed.
7. Repeat this step for each deck you want tags for.

### HARD MODE
I may have gotten lucky when I went through these steps to write this, and you might not actually find a pointer that works for all decks.

1. Use deck 1 for this, and search for the title, as we can find a pointer that can be used for all decks.
2. Repeat step 1 from ez mode on each address, keeping all pointer scan windows open.
3. Repeat steps 2-4 of ez mode taking but for all pointer scan windows. Do this until you have a decently small list of possible pointers.
4. Load songs into all 4 decks.
5. Switch between 2-deck and 4-deck mode, refreshing the pointer scan windows after each switch. Do this two or three times. You should be left with only a couple potentially valid pointers.
6. On the main CheatEngine window, click "Add Address Manually." In the window that pops up, check the "Pointer" box, set the Type to Text, and add as many offsets as is needed for whatever pointer you are checking. ![CheatEngine 10](img/ce-10.png)
7. Add the base address and offsets, verify the value is what you expect. ![CheatEngine 11](img/ce-11.png)
8. Increase the last non-0 offset by 0x8. Verify it shows the artist.             ![CheatEngine 12](img/ce-12.png)
9. Increase the offset by 0x30 from its initial value, and verify the song title in deck 2 appears. If it does not, discard the pointer and try the next one starting at step 7. 
10. Repeat step 9 cycling through all 4 decks, so the deck 4 title offset value will be 0x90 above the initial offset value. ![CheatEngine 13](img/ce-13.png)
11. Congratulations, you have a pointer that should work for all deck modes. Maybe.

## Master Deck
Finding a master deck pointer is easy as there are usually plenty of valid pointers for it. The master deck value is an integer with the deck indexes being 0-based. So deck 1 is set as master, the pointer will show 0, with deck 2 set it would show 1, etc.

1. You can change deck modes and the pointer found should still work, so to make things easier set Rekordbox to 4-deck mode
2. Set one of the decks in Rekordbox as Master.
3. In CheatEngine, set the Value Type to 4 bytes, enter the expected deck number in the search box, and click First Scan ![CheatEngine 14](img/ce-14.png)
4. Change the master deck in Rekordbox, set the search value to the expected one, and click Next Scan ![CheatEngine 15](img/ce-15.png)
5. Repeat step 4 until you have narrowed the list of address found to a manageable amount.
6. Change the master deck and check the addresses after each change. They should all show  the correct value. Choose one and scan for a pointer per Step 1 of ez mode artist/title instructions.
7. Choose any pointer you want that uses "rekordbox.exe" as part of the base address. You can sort the offset columns to find the pointer with the fewest offsets.