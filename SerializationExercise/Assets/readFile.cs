using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class readFile : MonoBehaviour
{
    // List of common English words
    string[] commonWords = { "the", "and", "you", "that", "day" };
    void Start()
    {
        deserialize();
    }

    void deserialize()
    {
        
        for (int i = 0; i < 29; i++)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream fs = File.OpenRead(Application.dataPath + "/clues/clue" + i + ".txt"))
            {
                fs.CopyTo(ms);

                BinaryReader reader = new BinaryReader(ms);
                ms.Seek(0, SeekOrigin.Begin);

                int newint = reader.ReadInt32();
                UnityEngine.Debug.Log("int " + newint.ToString());

                string newstring = reader.ReadString();
                
                // Decode the string using Caesar cipher
                //string decodedString = CaesarDecode(newstring, 3); // Assuming a shift of 3

                // Try decoding with different shifts (1 to 25)
                for (int shift = 1; shift <= 25; shift++)
                {
                    string decodedString = CaesarDecode(newstring, shift);
                    // Check if the decoded string contains common words
                    if (ContainsCommonWords(decodedString))
                    {
                        UnityEngine.Debug.Log($"Likely correct shift: {shift}, Decoded string: {decodedString}");
                        break; // Stop when the correct shift is found
                    }
                }

                
            }
        }
    }

    // Check if the decoded string contains common words
    bool ContainsCommonWords(string decodedString)
    {
        foreach (string word in commonWords)
        {
            if (decodedString.Contains(word))
            {
                return true;
            }
        }
        return false;
    }

    // Caesar Decode Method
    string CaesarDecode(string input, int shift)
    {
        char[] buffer = input.ToCharArray();
        for (int i = 0; i < buffer.Length; i++)
        {
            char letter = buffer[i];

            // Check if it's a letter before decoding
            if (char.IsLetter(letter))
            {
                // Decode uppercase letters
                if (char.IsUpper(letter))
                {
                    letter = (char)(letter - shift);

                    if (letter < 'A')
                    {
                        letter = (char)(letter + 26); // Wrap around the alphabet
                    }
                }
                // Decode lowercase letters
                else if (char.IsLower(letter))
                {
                    letter = (char)(letter - shift);

                    if (letter < 'a')
                    {
                        letter = (char)(letter + 26); // Wrap around the alphabet
                    }
                }
            }

            buffer[i] = letter;
        }
        return new string(buffer);
    }
}
