using nifly;
using System.Drawing;

//Source from Jampi0n's Github Respitory Skyrim-NifPatcher
//https://github.com/Jampi0n
//https://github.com/Jampi0n/Skyrim-NifPatcher
//https://github.com/Jampi0n/Skyrim-NifPatcher/blob/main/NifPatcher/NifWrapper.cs
//I changed the namespace and that was it
namespace ESLifyEverything.FormData
{
    public enum TextureId
    {
        Diffuse = 0,
        Normal = 1,
        EmissiveGlow = 2,
        Parallalx = 3,
        CubeMap = 4,
        Specular = 5,
        Subsurface = 6,
        Backlight = 7
    }

    public enum ShaderType
    {
        Default = 0,
        Environment_Map = 1,
        Glow_Shader = 2,
        Parallax = 3,
        Face_Tint = 4,
        Skin_Tint = 5,
        Hair_Tint = 6,
        Parallax_Occ = 7,
        Multitexture_Landscape = 8,
        LOD_Landscape = 9,
        Snow = 10,
        MultiLayer_Parallax = 11,
        Tree_Anim = 12,
        LOD_Objects = 13,
        Sparkle_Snow = 14,
        LOD_Objects_HD = 15,
        Eye_Envmap = 16,
        Cloud = 17,
        LOD_Landscape_Noise = 18,
        Multitexture_Landscape_LOD_Blend = 19,
        FO4_Dismemberment = 20
    }
    [Flags]
    public enum ShaderFlags1 : uint
    {
        None = 0,
        Specular = 1u << 0,
        Skinned = 1u << 1,
        Temp_Refraction = 1u << 2,
        Vertex_Alpha = 1u << 3,
        Greyscale_To_PaletteColor = 1u << 4,
        Greyscale_To_PaletteAlpha = 1u << 5,
        Use_Falloff = 1u << 6,
        Environment_Mapping = 1u << 7,
        Recieve_Shadows = 1u << 8,
        Cast_Shadows = 1u << 9,
        Facegen_Detail_Map = 1u << 10,
        Parallax = 1u << 11,
        Model_Space_Normals = 1u << 12,
        Non_Projective_Shadows = 1u << 13,
        Landscape = 1u << 14,
        Refraction = 1u << 15,
        Fire_Refraction = 1u << 16,
        Eye_Environment_Mapping = 1u << 17,
        Hair_Soft_Lighting = 1u << 18,
        Screendoor_Alpha_Fade = 1u << 19,
        Localmap_Hide_Secret = 1u << 20,
        FaceGen_RGB_Tint = 1u << 21,
        Own_Emit = 1u << 22,
        Projected_UV = 1u << 23,
        Multiple_Textures = 1u << 24,
        Remappable_Textures = 1u << 25,
        Decal = 1u << 26,
        Dynamic_Decal = 1u << 27,
        Parallax_Occlusion = 1u << 28,
        External_Emittance = 1u << 29,
        Soft_Effect = 1u << 30,
        ZBuffer_Test = 1u << 31,
    }
    [Flags]
    public enum ShaderFlags2 : uint
    {
        None = 0,
        ZBuffer_Write = 1u << 0,
        LOD_Landscape = 1u << 1,
        LOD_Objects = 1u << 2,
        No_Fade = 1u << 3,
        Double_Sided = 1u << 4,
        Vertex_Colors = 1u << 5,
        Glow_Map = 1u << 6,
        Assume_Shadowmask = 1u << 7,
        Packed_Tangent = 1u << 8,
        Multi_Index_Snow = 1u << 9,
        Vertex_Lighting = 1u << 10,
        Uniform_Scale = 1u << 11,
        Fit_Slope = 1u << 12,
        Billboard = 1u << 13,
        No_LOD_Land_Blend = 1u << 14,
        EnvMap_Light_Fade = 1u << 15,
        Wireframe = 1u << 16,
        Weapon_Blood = 1u << 17,
        Hide_On_Local_Map = 1u << 18,
        Premult_Alpha = 1u << 19,
        Cloud_LOD = 1u << 20,
        Anisotropic_Lighting = 1u << 21,
        No_Transparency_Multisampling = 1u << 22,
        Unused01 = 1u << 23,
        Multi_Layer_Parallax = 1u << 24,
        Soft_Lighting = 1u << 25,
        Rim_Lighting = 1u << 26,
        Back_Lighting = 1u << 27,
        Unused02 = 1u << 28,
        Tree_Anim = 1u << 29,
        Effect_Lighting = 1u << 30,
        HD_LOD_Objects = 1u << 31,
    }

    public class NifFileWrapper
    {
        protected NifFile nif;
        private readonly Dictionary<int, NiShapeWrapper> shapes = new();
        public NifFileWrapper(string path)
        {
            nif = new NifFile();
            nif.Load(path);

            int j = 0;
            for (int i = 0; i < nif.GetShapes().Count; ++i)
            {
                try
                {
                    shapes[j] = new NiShapeWrapper(this, nif.GetShapes()[i]);
                    j++;
                }
                catch (Exception) { }
            }
        }

        public bool SaveAs(string newPath, bool overwrite)
        {
            if (!overwrite)
            {
                if (File.Exists(newPath))
                {
                    return false;
                }
            }
            foreach (var shape in shapes.Values)
            {
                shape.Save();
            }
            // optimize or sortBlocks corrupts bound bow
            // references have wrong types and trying save in NifSkope fails

            // In general it's best to not reorder blocks, as that is not the purpose of this patcher anyway.
            nif.Save(newPath, new NifSaveOptions
            {
                optimize = false,
                sortBlocks = false
            })
            ;
            return true;
        }

        public int GetNumShapes()
        {
            return shapes.Count;
        }

        public NiShapeWrapper GetShape(int i)
        {
            return shapes[i];
        }


        public class NiShapeWrapper
        {
            protected NifFileWrapper nif;
            protected NiShape shape;
            protected BSLightingShaderProperty shader;
            protected string[] textures = new string[9];
            public NiShapeWrapper(NifFileWrapper nif, NiShape shape)
            {
                this.nif = nif;
                this.shape = shape;

                var blockCache = new niflycpp.BlockCache(nif.nif.GetHeader());

                var shaderRef = shape.ShaderPropertyRef();
                if (shape.HasShaderProperty() && !shaderRef.IsEmpty())
                {

                    BSShaderProperty? shaderProperty = niflycpp.BlockCache.SafeClone<BSShaderProperty>(blockCache.EditableBlockById<BSShaderProperty>(shaderRef.index));
                    if (shaderProperty is BSLightingShaderProperty lightingShader)
                    {
                        shader = lightingShader;
                    }
                }
                if (shader == null)
                {
                    throw new Exception("Cannot parse shader of nif.");
                }
                var textureSetRef = shader.TextureSetRef();
                if (niflycpp.BlockCache.SafeClone<BSShaderTextureSet>(blockCache.EditableBlockById<BSShaderTextureSet>(textureSetRef.index)) is BSShaderTextureSet textureSet)
                {
                    for (int i = 0; i < 9; ++i)
                    {
                        textures[i] = textureSet.textures.items()[i].get();
                    }
                }
                else
                {
                    throw new Exception("Cannot parse texture set of nif.");
                }
            }

            public void Save()
            {
                nif.nif.GetHeader().ReplaceBlock(shape.ShaderPropertyRef().index, shader);
            }

            public string GetTextureSlot(TextureId texIndex)
            {
                return textures[(int)texIndex];
            }
            public void SetTextureSlot(TextureId texIndex, string texture)
            {
                nif.nif.SetTextureSlot(shape, texture, (uint)texIndex);
                textures[(int)texIndex] = texture;
            }

            public string DiffuseMap
            {
                get => GetTextureSlot(TextureId.Diffuse);
                set => SetTextureSlot(TextureId.Diffuse, value);
            }

            public string NormalMap
            {
                get => GetTextureSlot(TextureId.Normal);
                set => SetTextureSlot(TextureId.Normal, value);
            }

            public string EmissiveGlowMap
            {
                get => GetTextureSlot(TextureId.EmissiveGlow);
                set => SetTextureSlot(TextureId.EmissiveGlow, value);
            }

            public string ParallalxMap
            {
                get => GetTextureSlot(TextureId.Parallalx);
                set => SetTextureSlot(TextureId.Parallalx, value);
            }

            public string CubeMap
            {
                get => GetTextureSlot(TextureId.CubeMap);
                set => SetTextureSlot(TextureId.CubeMap, value);
            }

            public string SpecularMap
            {
                get => GetTextureSlot(TextureId.Specular);
                set => SetTextureSlot(TextureId.Specular, value);
            }

            public string SubsurfaceMap
            {
                get => GetTextureSlot(TextureId.Subsurface);
                set => SetTextureSlot(TextureId.Subsurface, value);
            }

            public string BacklightMap
            {
                get => GetTextureSlot(TextureId.Backlight);
                set => SetTextureSlot(TextureId.Backlight, value);
            }

            public ShaderType ShaderType
            {
                get => (ShaderType)shader.GetShaderType();
                set => shader.SetShaderType((uint)value);
            }

            public ShaderFlags1 ShaderFlags1
            {
                get => (ShaderFlags1)shader.shaderFlags1;
                set => shader.shaderFlags1 = (uint)value;
            }

            public ShaderFlags2 ShaderFlags2
            {
                get => (ShaderFlags2)shader.shaderFlags2;
                set => shader.shaderFlags2 = (uint)value;
            }

            public void AddShaderFlags1(ShaderFlags1 flags)
            {
                ShaderFlags1 |= flags;
            }

            public void RemoveShaderFlags1(ShaderFlags1 flags)
            {
                ShaderFlags1 &= ~flags;
            }

            public bool HasAllFlags1(ShaderFlags1 flags)
            {
                return (ShaderFlags1 & flags) == flags;
            }

            public bool HasNoFlags1(ShaderFlags1 flags)
            {
                return (ShaderFlags1 & flags) == 0;
            }

            public void AddShaderFlags2(ShaderFlags2 flags)
            {
                ShaderFlags2 |= flags;
            }

            public bool HasAllFlags2(ShaderFlags2 flags)
            {
                return (ShaderFlags2 & flags) == flags;
            }

            public bool HasNoFlags2(ShaderFlags2 flags)
            {
                return (ShaderFlags2 & flags) == 0;
            }

            public void RemoveShaderFlags2(ShaderFlags2 flags)
            {
                ShaderFlags2 &= ~flags;
            }

            private static Color ConvertColor(Vector3 color)
            {
                return Color.FromArgb((int)Math.Round(255 * color.x), (int)Math.Round(255 * color.y), (int)Math.Round(255 * color.z));
            }

            private static Vector3 ConvertColor(Color color)
            {
                return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
            }

            public Color EmissiveColor
            {
                get => ConvertColor(shader.emissiveColor);
                set => shader.emissiveColor = ConvertColor(value);
            }

            public float EmissiveMultiple
            {
                get => shader.emissiveMultiple;
                set => shader.emissiveMultiple = value;
            }

            public float Alpha
            {
                get => shader.alpha;
                set => shader.alpha = value;
            }

            public float RefractionStrength
            {
                get => shader.refractionStrength;
                set => shader.refractionStrength = value;
            }

            public float Glossiness
            {
                get => shader.glossiness;
                set => shader.glossiness = value;
            }

            public Color SpecularColor
            {
                get => ConvertColor(shader.specularColor);
                set => shader.specularColor = ConvertColor(value);
            }

            public float SpecularStrength
            {
                get => shader.specularStrength;
                set => shader.specularStrength = value;
            }

            public float LightingEffect1
            {
                get => shader.softlighting;
                set => shader.softlighting = value;
            }

            public float LightingEffect2
            {
                get => shader.rimlightPower;
                set => shader.rimlightPower = value;
            }

            public float ParallaxInnerLayerThickness
            {
                get => shader.parallaxInnerLayerThickness;
                set => shader.parallaxInnerLayerThickness = value;
            }

            public float ParallaxRefractionScale
            {
                get => shader.parallaxRefractionScale;
                set => shader.parallaxRefractionScale = value;
            }

            public float ParallaxInnerLayerTextureScaleX
            {
                get => shader.parallaxInnerLayerTextureScale.u;
                set => shader.parallaxInnerLayerTextureScale.u = value;
            }

            public float ParallaxInnerLayerTextureScaleY
            {
                get => shader.parallaxInnerLayerTextureScale.v;
                set => shader.parallaxInnerLayerTextureScale.v = value;
            }

            public float ParallaxEnvmapStrength
            {
                get => shader.parallaxEnvmapStrength;
                set => shader.parallaxEnvmapStrength = value;
            }

            public float EnvironmentMapScale
            {
                get => shader.environmentMapScale;
                set => shader.environmentMapScale = value;
            }
        }
    }

}