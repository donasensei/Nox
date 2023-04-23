using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("characterName", "characterDesc", "characterImage", "characterStat", "currentHealth", "currentMana", "level", "experience", "characterSkill")]
	public class ES3UserType_CharacterData : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CharacterData() : base(typeof(CharacterData)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (CharacterData)obj;
			
			writer.WriteProperty("characterName", instance.characterName, ES3Type_string.Instance);
			writer.WriteProperty("characterDesc", instance.characterDesc, ES3Type_string.Instance);
			writer.WritePropertyByRef("characterImage", instance.characterImage);
			writer.WriteProperty("characterStat", instance.characterStat, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(CharacterStat)));
			writer.WriteProperty("currentHealth", instance.currentHealth, ES3Type_int.Instance);
			writer.WriteProperty("currentMana", instance.currentMana, ES3Type_int.Instance);
			writer.WriteProperty("level", instance.level, ES3Type_int.Instance);
			writer.WriteProperty("experience", instance.experience, ES3Type_int.Instance);
			writer.WriteProperty("characterSkill", instance.characterSkill, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<SkillData>)));
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (CharacterData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "characterName":
						instance.characterName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "characterDesc":
						instance.characterDesc = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "characterImage":
						instance.characterImage = reader.Read<UnityEngine.Sprite>(ES3Type_Sprite.Instance);
						break;
					case "characterStat":
						instance.characterStat = reader.Read<CharacterStat>();
						break;
					case "currentHealth":
						instance.currentHealth = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "currentMana":
						instance.currentMana = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "level":
						instance.level = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "experience":
						instance.experience = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "characterSkill":
						instance.characterSkill = reader.Read<System.Collections.Generic.List<SkillData>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_CharacterDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CharacterDataArray() : base(typeof(CharacterData[]), ES3UserType_CharacterData.Instance)
		{
			Instance = this;
		}
	}
}