use byteorder::{LittleEndian, ReadBytesExt, WriteBytesExt};
use indexmap::IndexMap;
use serde::{Deserialize, Serialize};
use std::fmt::{Display, Formatter};
use std::io::{Read, Result, Write};

#[derive(Debug, Serialize, Deserialize)]
#[serde(transparent)]
pub struct Loc {
    alltexts: IndexMap<u32, IndexMap<String, String>>,
}

impl Loc {
    pub fn from_text_loc(mut text_loc_file: impl Read) -> Result<Self> {
        let num_langs = text_loc_file.read_u16::<LittleEndian>()?;
        let mut languages: Vec<String> = Vec::with_capacity(num_langs as usize);

        for _ in 0..num_langs {
            let mut array = [0u8; 2];

            text_loc_file.read_exact(&mut array)?;
            languages.push(String::from_utf8_lossy(&array).into());
        }

        let num_strings = text_loc_file.read_u16::<LittleEndian>()?;
        let mut alltexts: IndexMap<u32, IndexMap<String, String>> = IndexMap::with_capacity(num_strings as usize);

        for _ in 0..num_strings {
            let id = text_loc_file.read_u32::<LittleEndian>()?;
            let mut texts: IndexMap<String, String> = IndexMap::with_capacity(num_langs as usize);

            for i in 0..num_langs {
                let string_length = text_loc_file.read_u16::<LittleEndian>()?;
                let mut vec: Vec<u16> = Vec::with_capacity(string_length as usize);

                for _ in 0..string_length {
                    vec.push(text_loc_file.read_u16::<LittleEndian>()?);
                }

                texts.insert(
                    languages[i as usize].clone(),
                    String::from_utf16_lossy(&vec[..(string_length - 1/* Without null terminator */) as usize]),
                );
            }

            alltexts.insert(id, texts);
        }

        Ok(Loc { alltexts })
    }

    pub fn from_json(json_file: impl Read) -> serde_json::Result<Self> {
        serde_json::from_reader(json_file)
    }

    pub fn save_json(&self, json_file: impl Write) -> serde_json::Result<()> {
        serde_json::to_writer_pretty(json_file, self)
    }

    pub fn save_text_loc(&self, mut text_loc_file: impl Write) -> Result<()> {
        let langtexts = self.alltexts.values().collect::<Vec<_>>();
        let Some(first_element) = langtexts.first() else { return Ok(()) };
        let first_element = *first_element;

        // Write languages count
        text_loc_file.write_u16::<LittleEndian>(first_element.len() as u16)?;

        let languages = first_element.keys().collect::<Vec<_>>();

        for language in languages {
            // Write language code
            text_loc_file.write_all(language.as_bytes())?;
        }

        // Write texts count
        text_loc_file.write_u16::<LittleEndian>(langtexts.len() as u16)?;

        for (id, langtext) in &self.alltexts {
            // Write ID
            text_loc_file.write_u32::<LittleEndian>(*id)?;

            for (_, text) in langtext {
                let mut u16text = text.encode_utf16().collect::<Vec<_>>();

                // Append null terminator
                u16text.push(0);

                // Write text length
                text_loc_file.write_u16::<LittleEndian>(u16text.len() as u16)?;

                // Write text
                for codepoint in u16text {
                    text_loc_file.write_u16::<LittleEndian>(codepoint)?;
                }
            }
        }

        Ok(())
    }
}

impl Display for Loc {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        f.write_str(&serde_json::to_string_pretty(self).map_err(|_| std::fmt::Error)?)
    }
}
