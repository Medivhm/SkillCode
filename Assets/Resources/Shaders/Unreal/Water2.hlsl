#define NUM_TEX_COORD_INTERPOLATORS 0
#define NUM_MATERIAL_TEXCOORDS_VERTEX 0
#define NUM_CUSTOM_VERTEX_INTERPOLATORS 0

struct Input
{
	//float3 Normal;
	float2 uv_MainTex : TEXCOORD0;
	float2 uv2_Material_Texture2D_0 : TEXCOORD1;
	float4 color : COLOR;
	float4 tangent;
	//float4 normal;
	float3 viewDir;
	float4 screenPos;
	float3 worldPos;
	//float3 worldNormal;
	float3 normal2;
};
struct SurfaceOutputStandard
{
	float3 Albedo;		// base (diffuse or specular) color
	float3 Normal;		// tangent space normal, if written
	half3 Emission;
	half Metallic;		// 0=non-metal, 1=metal
	// Smoothness is the user facing name, it should be perceptual smoothness but user should not have to deal with it.
	// Everywhere in the code you meet smoothness it is perceptual smoothness
	half Smoothness;	// 0=rough, 1=smooth
	half Occlusion;		// occlusion (default 1)
	float Alpha;		// alpha for transparencies
};

//#define HDRP 1
#define URP 1
#define UE5
//#define HAS_CUSTOMIZED_UVS 1
#define MATERIAL_TANGENTSPACENORMAL 1
//struct Material
//{
	//samplers start
SAMPLER( SamplerState_Linear_Repeat );
SAMPLER( SamplerState_Linear_Clamp );
TEXTURE2D(       Material_Texture2D_0 );
SAMPLER(  samplerMaterial_Texture2D_0 );
float4 Material_Texture2D_0_TexelSize;
float4 Material_Texture2D_0_ST;

//};

#ifdef UE5
	#define UE_LWC_RENDER_TILE_SIZE			2097152.0
	#define UE_LWC_RENDER_TILE_SIZE_SQRT	1448.15466
	#define UE_LWC_RENDER_TILE_SIZE_RSQRT	0.000690533954
	#define UE_LWC_RENDER_TILE_SIZE_RCP		4.76837158e-07
	#define UE_LWC_RENDER_TILE_SIZE_FMOD_PI		0.673652053
	#define UE_LWC_RENDER_TILE_SIZE_FMOD_2PI	0.673652053
	#define INVARIANT(X) X
	//#define PI 					(3.1415926535897932)

	#include "LargeWorldCoordinates.hlsl"
#endif
struct MaterialStruct
{
	float4 PreshaderBuffer[16];
	float4 ScalarExpressions[1];
	float VTPackedPageTableUniform[2];
	float VTPackedUniform[1];
};
static SamplerState View_MaterialTextureBilinearWrapedSampler;
static SamplerState View_MaterialTextureBilinearClampedSampler;
struct ViewStruct
{
	float GameTime;
	float RealTime;
	float DeltaTime;
	float PrevFrameGameTime;
	float PrevFrameRealTime;
	float MaterialTextureMipBias;	
	float4 PrimitiveSceneData[ 40 ];
	float4 TemporalAAParams;
	float2 ViewRectMin;
	float4 ViewSizeAndInvSize;
	float MaterialTextureDerivativeMultiply;
	uint StateFrameIndexMod8;
	float FrameNumber;
	float2 FieldOfViewWideAngles;
	float4 RuntimeVirtualTextureMipLevel;
	float PreExposure;
	float4 BufferBilinearUVMinMax;
};
struct ResolvedViewStruct
{
	#ifdef UE5
		FLWCVector3 WorldCameraOrigin;
		FLWCVector3 PrevWorldCameraOrigin;
		FLWCVector3 PreViewTranslation;
		FLWCVector3 WorldViewOrigin;
	#else
		float3 WorldCameraOrigin;
		float3 PrevWorldCameraOrigin;
		float3 PreViewTranslation;
		float3 WorldViewOrigin;
	#endif
	float4 ScreenPositionScaleBias;
	float4x4 TranslatedWorldToView;
	float4x4 TranslatedWorldToCameraView;
	float4x4 TranslatedWorldToClip;
	float4x4 ViewToTranslatedWorld;
	float4x4 PrevViewToTranslatedWorld;
	float4x4 CameraViewToTranslatedWorld;
	float4 BufferBilinearUVMinMax;
	float4 XRPassthroughCameraUVs[ 2 ];
};
struct PrimitiveStruct
{
	float4x4 WorldToLocal;
	float4x4 LocalToWorld;
};

static ViewStruct View;
static ResolvedViewStruct ResolvedView;
static PrimitiveStruct Primitive;
uniform float4 View_BufferSizeAndInvSize;
uniform float4 LocalObjectBoundsMin;
uniform float4 LocalObjectBoundsMax;
static SamplerState Material_Wrap_WorldGroupSettings;
static SamplerState Material_Clamp_WorldGroupSettings;

#include "UnrealCommon.cginc"

static MaterialStruct Material;
void InitializeExpressions()
{
	Material.PreshaderBuffer[0] = float4(0.000000,0.000000,0.000000,0.000000);//(Unknown)
	Material.PreshaderBuffer[1] = float4(980.000000,4.401936,2.098079,8.030600);//(Unknown)
	Material.PreshaderBuffer[2] = float4(2.833831,16.932167,4.114872,0.708801);//(Unknown)
	Material.PreshaderBuffer[3] = float4(1.122439,0.000000,0.000000,0.000000);//(Unknown)
	Material.PreshaderBuffer[4] = float4(0.465382,0.723958,0.192301,1.000000);//(Unknown)
	Material.PreshaderBuffer[5] = float4(0.697917,0.697917,0.697917,1.000000);//(Unknown)
	Material.PreshaderBuffer[6] = float4(0.050890,0.348958,0.314118,1.000000);//(Unknown)
	Material.PreshaderBuffer[7] = float4(0.010330,0.026241,0.017642,1.000000);//(Unknown)
	Material.PreshaderBuffer[8] = float4(70.000000,0.000000,0.000000,70.000000);//(Unknown)
	Material.PreshaderBuffer[9] = float4(200.000000,0.005000,0.000000,0.000000);//(Unknown)
	Material.PreshaderBuffer[10] = float4(0.010330,0.026241,0.017642,0.000000);//(Unknown)
	Material.PreshaderBuffer[11] = float4(0.050890,0.348958,0.314118,0.000000);//(Unknown)
	Material.PreshaderBuffer[12] = float4(0.697917,0.697917,0.697917,0.000000);//(Unknown)
	Material.PreshaderBuffer[13] = float4(0.465382,0.723958,0.192301,0.000000);//(Unknown)
	Material.PreshaderBuffer[14] = float4(0.000000,0.000000,0.000000,100.000000);//(Unknown)
	Material.PreshaderBuffer[15] = float4(100.000000,0.010000,1.000000,0.000000);//(Unknown)
}
float3 GetMaterialWorldPositionOffset(FMaterialVertexParameters Parameters)
{
#if IS_NANITE_PASS
	BRANCH
		if (!Parameters.bEvaluateWorldPositionOffset)
		{
			return float3(0, 0, 0);
		}
#endif

#if USE_INSTANCING || USE_INSTANCE_CULLING
	// skip if this instance is hidden
	if (Parameters.PerInstanceParams.y < 1.f)
	{
		return float3(0, 0, 0);
	}
#endif

	if ((GetPrimitiveData(Parameters).Flags & PRIMITIVE_SCENE_DATA_FLAG_EVALUATE_WORLD_POSITION_OFFSET) == 0)
	{
		return float3(0, 0, 0);
	}

	return MaterialFloat3(0.00000000,0.00000000,0.00000000);;
}
void CalcPixelMaterialInputs(in out FMaterialPixelParameters Parameters, in out FPixelMaterialInputs PixelMaterialInputs)
{
	//WorldAligned texturing & others use normals & stuff that think Z is up
	Parameters.TangentToWorld[0] = Parameters.TangentToWorld[0].xzy;
	Parameters.TangentToWorld[1] = Parameters.TangentToWorld[1].xzy;
	Parameters.TangentToWorld[2] = Parameters.TangentToWorld[2].xzy;

	float3 WorldNormalCopy = Parameters.WorldNormal;

	// Initial calculations (required for Normal)
	FLWCVector3 Local0 = GetWorldPosition_NoMaterialOffsets(Parameters);
	FLWCScalar Local1 = LWCDot(DERIV_BASE_VALUE(Local0), LWCPromote((normalize(MaterialFloat3(-1.00000000,0.40000001,0.00000000)) * ((3.14159203 * 2.00000000) / 1398.82092285))));
	MaterialFloat Local2 = (Material.PreshaderBuffer[1].z * View.GameTime);
	FLWCScalar Local3 = LWCSubtract(DERIV_BASE_VALUE(Local1), LWCPromote(Local2));
	FLWCScalar Local4 = LWCMultiply(DERIV_BASE_VALUE(Local3), LWCPromote(1.00000012));
	MaterialFloat Local5 = LWCSin(DERIV_BASE_VALUE(Local4));
	MaterialFloat Local6 = (DERIV_BASE_VALUE(Local5) * (4.00000000 * ((3.14159203 * 2.00000000) / 1398.82092285)));
	MaterialFloat3 Local7 = (((MaterialFloat3)DERIV_BASE_VALUE(Local6)) * normalize(MaterialFloat3(-1.00000000,0.40000001,0.00000000)));
	MaterialFloat2 Local8 = DERIV_BASE_VALUE(Local7).rg;
	MaterialFloat Local9 = LWCCos(DERIV_BASE_VALUE(Local4));
	MaterialFloat Local10 = (DERIV_BASE_VALUE(Local9) * (4.00000000 * ((3.14159203 * 2.00000000) / 1398.82092285)));
	MaterialFloat Local11 = (DERIV_BASE_VALUE(Local10) * (0.10000000 / (4.00000000 * ((3.14159203 * 2.00000000) / 1398.82092285))));
	MaterialFloat Local12 = (1.00000000 - DERIV_BASE_VALUE(Local11));
	MaterialFloat3 Local13 = MaterialFloat3(DERIV_BASE_VALUE(Local8),DERIV_BASE_VALUE(Local12));
	MaterialFloat2 Local14 = DERIV_BASE_VALUE(Local13).rg;
	MaterialFloat Local15 = DERIV_BASE_VALUE(Local13).b;
	MaterialFloat Local16 = (DERIV_BASE_VALUE(Local15) + 1.00000000);
	MaterialFloat3 Local17 = MaterialFloat3(DERIV_BASE_VALUE(Local14),DERIV_BASE_VALUE(Local16));
	FLWCScalar Local18 = LWCDot(DERIV_BASE_VALUE(Local0), LWCPromote((normalize(MaterialFloat3(-0.98145300,-0.47884801,0.00000000)) * ((3.14159203 * 2.00000000) / 766.75726318))));
	MaterialFloat Local19 = (Material.PreshaderBuffer[2].x * View.GameTime);
	FLWCScalar Local20 = LWCSubtract(DERIV_BASE_VALUE(Local18), LWCPromote(Local19));
	FLWCScalar Local21 = LWCMultiply(DERIV_BASE_VALUE(Local20), LWCPromote(1.00000012));
	MaterialFloat Local22 = LWCSin(DERIV_BASE_VALUE(Local21));
	MaterialFloat Local23 = (DERIV_BASE_VALUE(Local22) * (5.88187695 * ((3.14159203 * 2.00000000) / 766.75726318)));
	MaterialFloat3 Local24 = (((MaterialFloat3)DERIV_BASE_VALUE(Local23)) * normalize(MaterialFloat3(-0.98145300,-0.47884801,0.00000000)));
	MaterialFloat2 Local25 = DERIV_BASE_VALUE(Local24).rg;
	MaterialFloat Local26 = LWCCos(DERIV_BASE_VALUE(Local21));
	MaterialFloat Local27 = (DERIV_BASE_VALUE(Local26) * (5.88187695 * ((3.14159203 * 2.00000000) / 766.75726318)));
	MaterialFloat Local28 = (DERIV_BASE_VALUE(Local27) * (0.17000000 / (5.88187695 * ((3.14159203 * 2.00000000) / 766.75726318))));
	MaterialFloat Local29 = (1.00000000 - DERIV_BASE_VALUE(Local28));
	MaterialFloat3 Local30 = MaterialFloat3(DERIV_BASE_VALUE(Local25),DERIV_BASE_VALUE(Local29));
	MaterialFloat2 Local31 = DERIV_BASE_VALUE(Local30).rg;
	MaterialFloat2 Local32 = (DERIV_BASE_VALUE(Local31) * ((MaterialFloat2)-1.00000000));
	MaterialFloat Local33 = DERIV_BASE_VALUE(Local30).b;
	MaterialFloat3 Local34 = MaterialFloat3(DERIV_BASE_VALUE(Local32),DERIV_BASE_VALUE(Local33));
	MaterialFloat Local35 = dot(DERIV_BASE_VALUE(Local17),DERIV_BASE_VALUE(Local34));
	MaterialFloat3 Local36 = (DERIV_BASE_VALUE(Local17) * ((MaterialFloat3)DERIV_BASE_VALUE(Local35)));
	MaterialFloat3 Local37 = (((MaterialFloat3)DERIV_BASE_VALUE(Local16)) * DERIV_BASE_VALUE(Local34));
	MaterialFloat3 Local38 = (DERIV_BASE_VALUE(Local36) - DERIV_BASE_VALUE(Local37));
	MaterialFloat2 Local39 = DERIV_BASE_VALUE(Local38).rg;
	MaterialFloat Local40 = DERIV_BASE_VALUE(Local38).b;
	MaterialFloat Local41 = (DERIV_BASE_VALUE(Local40) + 1.00000000);
	MaterialFloat3 Local42 = MaterialFloat3(DERIV_BASE_VALUE(Local39),DERIV_BASE_VALUE(Local41));
	FLWCScalar Local43 = LWCDot(DERIV_BASE_VALUE(Local0), LWCPromote((normalize(MaterialFloat3(0.46000001,-0.83999997,0.00000000)) * ((3.14159203 * 2.00000000) / 363.65814209))));
	MaterialFloat Local44 = (Material.PreshaderBuffer[2].z * View.GameTime);
	FLWCScalar Local45 = LWCSubtract(DERIV_BASE_VALUE(Local43), LWCPromote(Local44));
	FLWCScalar Local46 = LWCMultiply(DERIV_BASE_VALUE(Local45), LWCPromote(1.00000012));
	MaterialFloat Local47 = LWCSin(DERIV_BASE_VALUE(Local46));
	MaterialFloat Local48 = (DERIV_BASE_VALUE(Local47) * (1.60000002 * ((3.14159203 * 2.00000000) / 363.65814209)));
	MaterialFloat3 Local49 = (((MaterialFloat3)DERIV_BASE_VALUE(Local48)) * normalize(MaterialFloat3(0.46000001,-0.83999997,0.00000000)));
	MaterialFloat2 Local50 = DERIV_BASE_VALUE(Local49).rg;
	MaterialFloat Local51 = LWCCos(DERIV_BASE_VALUE(Local46));
	MaterialFloat Local52 = (DERIV_BASE_VALUE(Local51) * (1.60000002 * ((3.14159203 * 2.00000000) / 363.65814209)));
	MaterialFloat Local53 = (DERIV_BASE_VALUE(Local52) * (0.73000002 / (1.60000002 * ((3.14159203 * 2.00000000) / 363.65814209))));
	MaterialFloat Local54 = (1.00000000 - DERIV_BASE_VALUE(Local53));
	MaterialFloat3 Local55 = MaterialFloat3(DERIV_BASE_VALUE(Local50),DERIV_BASE_VALUE(Local54));
	MaterialFloat2 Local56 = DERIV_BASE_VALUE(Local55).rg;
	MaterialFloat2 Local57 = (DERIV_BASE_VALUE(Local56) * ((MaterialFloat2)-1.00000000));
	MaterialFloat Local58 = DERIV_BASE_VALUE(Local55).b;
	MaterialFloat3 Local59 = MaterialFloat3(DERIV_BASE_VALUE(Local57),DERIV_BASE_VALUE(Local58));
	MaterialFloat Local60 = dot(DERIV_BASE_VALUE(Local42),DERIV_BASE_VALUE(Local59));
	MaterialFloat3 Local61 = (DERIV_BASE_VALUE(Local42) * ((MaterialFloat3)DERIV_BASE_VALUE(Local60)));
	MaterialFloat3 Local62 = (((MaterialFloat3)DERIV_BASE_VALUE(Local41)) * DERIV_BASE_VALUE(Local59));
	MaterialFloat3 Local63 = (DERIV_BASE_VALUE(Local61) - DERIV_BASE_VALUE(Local62));
	MaterialFloat2 Local64 = DERIV_BASE_VALUE(Local63).rg;
	MaterialFloat Local65 = DERIV_BASE_VALUE(Local63).b;
	MaterialFloat Local66 = (DERIV_BASE_VALUE(Local65) + 1.00000000);
	MaterialFloat3 Local67 = MaterialFloat3(DERIV_BASE_VALUE(Local64),DERIV_BASE_VALUE(Local66));
	MaterialFloat Local68 = (View.GameTime * MaterialFloat2(17.50000000,8.50000000).r);
	MaterialFloat Local69 = (View.GameTime * MaterialFloat2(17.50000000,8.50000000).g);
	FLWCVector3 Local70 = GetWorldPosition(Parameters);
	FLWCVector2 Local71 = MakeLWCVector(LWCGetX(DERIV_BASE_VALUE(Local70)), LWCGetY(DERIV_BASE_VALUE(Local70)));
	FLWCVector2 Local72 = LWCAdd(LWCPromote(MaterialFloat2(Local68,Local69)), DERIV_BASE_VALUE(Local71));
	FLWCVector2 Local73 = LWCMultiply(DERIV_BASE_VALUE(Local72), LWCPromote(((MaterialFloat2)0.00350000)));
	MaterialFloat2 Local74 = LWCApplyAddressMode(DERIV_BASE_VALUE(Local73), LWCADDRESSMODE_WRAP, LWCADDRESSMODE_WRAP);
	MaterialFloat Local75 = MaterialStoreTexCoordScale(Parameters, Local74, 0);
	MaterialFloat4 Local76 = UnpackNormalMap(Texture2DSampleBias(Material_Texture2D_0,samplerMaterial_Texture2D_0,Local74,View.MaterialTextureMipBias));
	MaterialFloat Local77 = MaterialStoreTexSample(Parameters, Local76, 0);
	MaterialFloat Local78 = (View.GameTime * MaterialFloat2(0.25000000,-15.00000000).r);
	MaterialFloat Local79 = (View.GameTime * MaterialFloat2(0.25000000,-15.00000000).g);
	FLWCVector2 Local80 = LWCAdd(LWCPromote(MaterialFloat2(Local78,Local79)), DERIV_BASE_VALUE(Local71));
	FLWCVector2 Local81 = LWCMultiply(DERIV_BASE_VALUE(Local80), LWCPromote(((MaterialFloat2)0.00030000)));
	MaterialFloat2 Local82 = LWCApplyAddressMode(DERIV_BASE_VALUE(Local81), LWCADDRESSMODE_WRAP, LWCADDRESSMODE_WRAP);
	MaterialFloat Local83 = MaterialStoreTexCoordScale(Parameters, Local82, 0);
	MaterialFloat4 Local84 = UnpackNormalMap(Texture2DSampleBias(Material_Texture2D_0,samplerMaterial_Texture2D_0,Local82,View.MaterialTextureMipBias));
	MaterialFloat Local85 = MaterialStoreTexSample(Parameters, Local84, 0);
	MaterialFloat2 Local86 = (MaterialFloat2(Local76.r,Local76.g) + MaterialFloat2(Local84.r,Local84.g));
	MaterialFloat2 Local87 = (Local86 * ((MaterialFloat2)Material.PreshaderBuffer[2].w));
	MaterialFloat Local88 = (View.GameTime * MaterialFloat2(2.15000010,2.79999995).r);
	MaterialFloat Local89 = (View.GameTime * MaterialFloat2(2.15000010,2.79999995).g);
	FLWCVector2 Local90 = LWCAdd(LWCPromote(MaterialFloat2(Local88,Local89)), DERIV_BASE_VALUE(Local71));
	FLWCVector2 Local91 = LWCMultiply(DERIV_BASE_VALUE(Local90), LWCPromote(MaterialFloat2(0.00010000,0.00040000)));
	MaterialFloat2 Local92 = LWCApplyAddressMode(DERIV_BASE_VALUE(Local91), LWCADDRESSMODE_WRAP, LWCADDRESSMODE_WRAP);
	MaterialFloat Local93 = MaterialStoreTexCoordScale(Parameters, Local92, 0);
	MaterialFloat4 Local94 = UnpackNormalMap(Texture2DSampleBias(Material_Texture2D_0,samplerMaterial_Texture2D_0,Local92,View.MaterialTextureMipBias));
	MaterialFloat Local95 = MaterialStoreTexSample(Parameters, Local94, 0);
	MaterialFloat2 Local96 = (MaterialFloat2(Local94.r,Local94.g) * ((MaterialFloat2)Material.PreshaderBuffer[3].x));
	MaterialFloat2 Local97 = (Local87 + Local96);
	MaterialFloat2 Local98 = (Local97 * Local97);
	MaterialFloat Local99 = (1.00000000 - Local98.r);
	MaterialFloat Local100 = (Local99 - Local98.g);
	MaterialFloat Local101 = sqrt(Local100);
	MaterialFloat Local102 = (Local101 * 1.00000000);
	MaterialFloat Local103 = dot(MaterialFloat3(Local97,Local102),MaterialFloat3(Local97,Local102));
	MaterialFloat3 Local104 = normalize(MaterialFloat3(Local97,Local102));
	MaterialFloat4 Local105 = ((abs(Local103 - 0.00000100) > 0.00001000) ? ((Local103 >= 0.00000100) ? MaterialFloat4(Local104,0.00000000) : MaterialFloat4(MaterialFloat3(0.00000000,0.00000000,1.00000000),1.00000000)) : MaterialFloat4(MaterialFloat3(0.00000000,0.00000000,1.00000000),1.00000000));
	MaterialFloat2 Local106 = (Local105.rgb.rg * ((MaterialFloat2)-1.00000000));
	MaterialFloat Local107 = dot(DERIV_BASE_VALUE(Local67),MaterialFloat3(Local106,Local105.rgb.b));
	MaterialFloat3 Local108 = (DERIV_BASE_VALUE(Local67) * ((MaterialFloat3)Local107));
	MaterialFloat3 Local109 = (((MaterialFloat3)DERIV_BASE_VALUE(Local66)) * MaterialFloat3(Local106,Local105.rgb.b));
	MaterialFloat3 Local110 = (Local108 - Local109);

	// The Normal is a special case as it might have its own expressions and also be used to calculate other inputs, so perform the assignment here
	PixelMaterialInputs.Normal = Local110;


#if TEMPLATE_USES_STRATA
	Parameters.StrataPixelFootprint = StrataGetPixelFootprint(Parameters.WorldPosition_CamRelative);
	Parameters.SharedLocalBases = StrataInitialiseSharedLocalBases();
	Parameters.StrataTree = GetInitialisedStrataTree();
#endif

	// Note that here MaterialNormal can be in world space or tangent space
	float3 MaterialNormal = GetMaterialNormal(Parameters, PixelMaterialInputs);

#if MATERIAL_TANGENTSPACENORMAL

#if FEATURE_LEVEL >= FEATURE_LEVEL_SM4
	// Mobile will rely on only the final normalize for performance
	MaterialNormal = normalize(MaterialNormal);
#endif

	// normalizing after the tangent space to world space conversion improves quality with sheared bases (UV layout to WS causes shrearing)
	// use full precision normalize to avoid overflows
	Parameters.WorldNormal = TransformTangentNormalToWorld(Parameters.TangentToWorld, MaterialNormal);

#else //MATERIAL_TANGENTSPACENORMAL

	Parameters.WorldNormal = normalize(MaterialNormal);

#endif //MATERIAL_TANGENTSPACENORMAL

#if MATERIAL_TANGENTSPACENORMAL
	// flip the normal for backfaces being rendered with a two-sided material
	Parameters.WorldNormal *= Parameters.TwoSidedSign;
#endif

	Parameters.ReflectionVector = ReflectionAboutCustomWorldNormal(Parameters, Parameters.WorldNormal, false);

#if !PARTICLE_SPRITE_FACTORY
	Parameters.Particle.MotionBlurFade = 1.0f;
#endif // !PARTICLE_SPRITE_FACTORY

	// Now the rest of the inputs
	FLWCVector3 Local111 = LWCSubtract(DERIV_BASE_VALUE(Local70), LWCPromote(Material.PreshaderBuffer[8].yzw));
	FLWCVector3 Local112 = ResolvedView.WorldCameraOrigin;
	FLWCVector3 Local113 = LWCSubtract(Local112, LWCPromote(Material.PreshaderBuffer[8].yzw));
	FLWCVector3 Local114 = LWCSubtract(DERIV_BASE_VALUE(Local111), Local113);
	MaterialFloat Local115 = CalcSceneDepth(ScreenAlignedPosition(GetScreenPosition(Parameters)));
	MaterialFloat Local116 = GetPixelDepth(Parameters);
	MaterialFloat Local117 = DERIV_BASE_VALUE(Local116).r;
	MaterialFloat Local118 = (Local115.r / DERIV_BASE_VALUE(Local117));
	FLWCVector3 Local119 = LWCMultiply(DERIV_BASE_VALUE(Local114), LWCPromote(((MaterialFloat3)Local118)));
	FLWCVector3 Local120 = LWCAdd(Local119, Local113);
	FLWCScalar Local121 = LWCSubtract(LWCPromote(1.00000000), LWCGetZ(Local120));
	FLWCScalar Local122 = LWCMultiply(Local121, LWCPromote(Material.PreshaderBuffer[9].y));
	MaterialFloat Local123 = PositiveClampedPow(LWCToFloat(Local122),1.00000000);
	MaterialFloat Local124 = dot(WorldNormalCopy,Parameters.CameraVector);
	MaterialFloat Local125 = max(0.00000000,Local124);
	MaterialFloat Local126 = (1.00000000 - Local125);
	MaterialFloat Local127 = abs(Local126);
	MaterialFloat Local128 = max(Local127,0.00010000);
	MaterialFloat Local129 = PositiveClampedPow(Local128,3.00000000);
	MaterialFloat Local130 = (Local129 * (1.00000000 - 0.04000000));
	MaterialFloat Local131 = (Local130 + 0.04000000);
	MaterialFloat Local132 = (Local123 + Local131);
	MaterialFloat Local133 = saturate(Local132);
	MaterialFloat3 Local134 = lerp(Material.PreshaderBuffer[11].xyz,Material.PreshaderBuffer[10].xyz,Local133);
	MaterialFloat4 Local135 = Parameters.VertexColor;
	MaterialFloat Local136 = DERIV_BASE_VALUE(Local135).r;
	MaterialFloat3 Local137 = lerp(Material.PreshaderBuffer[12].xyz,Local134,DERIV_BASE_VALUE(Local136));
	MaterialFloat Local138 = DERIV_BASE_VALUE(Local135).g;
	MaterialFloat3 Local139 = lerp(Material.PreshaderBuffer[13].xyz,Local137,DERIV_BASE_VALUE(Local138));
	MaterialFloat3 Local140 = (Local139 * ((MaterialFloat3)0.05000000));
	MaterialFloat3 Local141 = lerp(Local140,Material.PreshaderBuffer[14].xyz,Material.PreshaderBuffer[13].w);
	MaterialFloat Local142 = lerp(0.40000001,0.00000000,DERIV_BASE_VALUE(Local136));
	MaterialFloat Local143 = lerp(0.60000002,DERIV_BASE_VALUE(Local142),DERIV_BASE_VALUE(Local138));
	MaterialFloat Local144 = (Local115 - DERIV_BASE_VALUE(Local116));
	MaterialFloat Local145 = (Local144 * Material.PreshaderBuffer[15].y);
	MaterialFloat Local146 = saturate(Local145);
	MaterialFloat Local147 = (1.00000000 * Local146);
	MaterialFloat Local148 = (Local133 * Local147);
	MaterialFloat Local149 = lerp(Material.PreshaderBuffer[15].z,Local148,DERIV_BASE_VALUE(Local136));
	MaterialFloat Local150 = lerp(Material.PreshaderBuffer[15].z,Local149,DERIV_BASE_VALUE(Local138));
	MaterialFloat Local151 = lerp(1.00000000,1.33000004,Local147);

	PixelMaterialInputs.EmissiveColor = Local141;
	PixelMaterialInputs.Opacity = Local150;
	PixelMaterialInputs.OpacityMask = 1.00000000;
	PixelMaterialInputs.BaseColor = Local139;
	PixelMaterialInputs.Metallic = 0.00000000;
	PixelMaterialInputs.Specular = 0.50000000;
	PixelMaterialInputs.Roughness = Local143;
	PixelMaterialInputs.Anisotropy = 0.00000000;
	PixelMaterialInputs.Normal = Local110;
	PixelMaterialInputs.Tangent = MaterialFloat3(1.00000000,0.00000000,0.00000000);
	PixelMaterialInputs.Subsurface = 0;
	PixelMaterialInputs.AmbientOcclusion = 1.00000000;
	PixelMaterialInputs.Refraction = MaterialFloat2(Local151,Material.PreshaderBuffer[15].w);
	PixelMaterialInputs.PixelDepthOffset = 0.00000000;
	PixelMaterialInputs.ShadingModel = 1;
	PixelMaterialInputs.FrontMaterial = GetInitialisedStrataData();


#if MATERIAL_USES_ANISOTROPY
	Parameters.WorldTangent = CalculateAnisotropyTangent(Parameters, PixelMaterialInputs);
#else
	Parameters.WorldTangent = 0;
#endif
}

#define UnityObjectToWorldDir TransformObjectToWorld

void SetupCommonData( int Parameters_PrimitiveId )
{
	View_MaterialTextureBilinearWrapedSampler = SamplerState_Linear_Repeat;
	View_MaterialTextureBilinearClampedSampler = SamplerState_Linear_Clamp;

	Material_Wrap_WorldGroupSettings = SamplerState_Linear_Repeat;
	Material_Clamp_WorldGroupSettings = SamplerState_Linear_Clamp;

	View.GameTime = View.RealTime = _Time.y;// _Time is (t/20, t, t*2, t*3)
	View.PrevFrameGameTime = View.GameTime - unity_DeltaTime.x;//(dt, 1/dt, smoothDt, 1/smoothDt)
	View.PrevFrameRealTime = View.RealTime;
	View.DeltaTime = unity_DeltaTime.x;
	View.MaterialTextureMipBias = 0.0;
	View.TemporalAAParams = float4( 0, 0, 0, 0 );
	View.ViewRectMin = float2( 0, 0 );
	View.ViewSizeAndInvSize = View_BufferSizeAndInvSize;
	View.MaterialTextureDerivativeMultiply = 1.0f;
	View.StateFrameIndexMod8 = 0;
	View.FrameNumber = (int)_Time.y;
	View.FieldOfViewWideAngles = float2( PI * 0.42f, PI * 0.42f );//75degrees, default unity
	View.RuntimeVirtualTextureMipLevel = float4( 0, 0, 0, 0 );
	View.PreExposure = 0;
	View.BufferBilinearUVMinMax = float4(
		View_BufferSizeAndInvSize.z * ( 0 + 0.5 ),//EffectiveViewRect.Min.X
		View_BufferSizeAndInvSize.w * ( 0 + 0.5 ),//EffectiveViewRect.Min.Y
		View_BufferSizeAndInvSize.z * ( View_BufferSizeAndInvSize.x - 0.5 ),//EffectiveViewRect.Max.X
		View_BufferSizeAndInvSize.w * ( View_BufferSizeAndInvSize.y - 0.5 ) );//EffectiveViewRect.Max.Y

	for( int i2 = 0; i2 < 40; i2++ )
		View.PrimitiveSceneData[ i2 ] = float4( 0, 0, 0, 0 );

	float4x4 LocalToWorld = transpose( UNITY_MATRIX_M );
    LocalToWorld[3] = float4(ToUnrealPos(LocalToWorld[3]), LocalToWorld[3].w);
	float4x4 WorldToLocal = transpose( UNITY_MATRIX_I_M );
	float4x4 ViewMatrix = transpose( UNITY_MATRIX_V );
	float4x4 InverseViewMatrix = transpose( UNITY_MATRIX_I_V );
	float4x4 ViewProjectionMatrix = transpose( UNITY_MATRIX_VP );
	uint PrimitiveBaseOffset = Parameters_PrimitiveId * PRIMITIVE_SCENE_DATA_STRIDE;
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 0 ] = LocalToWorld[ 0 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 1 ] = LocalToWorld[ 1 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 2 ] = LocalToWorld[ 2 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 3 ] = LocalToWorld[ 3 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 5 ] = float4( ToUnrealPos( SHADERGRAPH_OBJECT_POSITION ), 100.0 );//ObjectWorldPosition
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 6 ] = WorldToLocal[ 0 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 7 ] = WorldToLocal[ 1 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 8 ] = WorldToLocal[ 2 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 9 ] = WorldToLocal[ 3 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 10 ] = LocalToWorld[ 0 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 11 ] = LocalToWorld[ 1 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 12 ] = LocalToWorld[ 2 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 13 ] = LocalToWorld[ 3 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 18 ] = float4( ToUnrealPos( SHADERGRAPH_OBJECT_POSITION ), 0 );//ActorWorldPosition
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 19 ] = LocalObjectBoundsMax - LocalObjectBoundsMin;//ObjectBounds
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 21 ] = mul( LocalToWorld, float3( 1, 0, 0 ) );
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 23 ] = LocalObjectBoundsMin;//LocalObjectBoundsMin 
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 24 ] = LocalObjectBoundsMax;//LocalObjectBoundsMax

#ifdef UE5
	ResolvedView.WorldCameraOrigin = LWCPromote( ToUnrealPos( _WorldSpaceCameraPos.xyz ) );
	ResolvedView.PreViewTranslation = LWCPromote( float3( 0, 0, 0 ) );
	ResolvedView.WorldViewOrigin = LWCPromote( float3( 0, 0, 0 ) );
#else
	ResolvedView.WorldCameraOrigin = ToUnrealPos( _WorldSpaceCameraPos.xyz );
	ResolvedView.PreViewTranslation = float3( 0, 0, 0 );
	ResolvedView.WorldViewOrigin = float3( 0, 0, 0 );
#endif
	ResolvedView.PrevWorldCameraOrigin = ResolvedView.WorldCameraOrigin;
	ResolvedView.ScreenPositionScaleBias = float4( 1, 1, 0, 0 );
	ResolvedView.TranslatedWorldToView		 = ViewMatrix;
	ResolvedView.TranslatedWorldToCameraView = ViewMatrix;
	ResolvedView.TranslatedWorldToClip		 = ViewProjectionMatrix;
	ResolvedView.ViewToTranslatedWorld		 = InverseViewMatrix;
	ResolvedView.PrevViewToTranslatedWorld = ResolvedView.ViewToTranslatedWorld;
	ResolvedView.CameraViewToTranslatedWorld = InverseViewMatrix;
	ResolvedView.BufferBilinearUVMinMax = View.BufferBilinearUVMinMax;
	Primitive.WorldToLocal = WorldToLocal;
	Primitive.LocalToWorld = LocalToWorld;
}
#define VS_USES_UNREAL_SPACE 1
float3 PrepareAndGetWPO( float4 VertexColor, float3 UnrealWorldPos, float3 UnrealNormal, float4 InTangent,
						 float4 UV0, float4 UV1 )
{
	InitializeExpressions();
	FMaterialVertexParameters Parameters = (FMaterialVertexParameters)0;

	float3 InWorldNormal = UnrealNormal;
	float4 tangentWorld = InTangent;
	tangentWorld.xyz = normalize( tangentWorld.xyz );
	//float3x3 tangentToWorld = CreateTangentToWorldPerVertex( InWorldNormal, tangentWorld.xyz, tangentWorld.w );
	Parameters.TangentToWorld = float3x3( normalize( cross( InWorldNormal, tangentWorld.xyz ) * tangentWorld.w ), tangentWorld.xyz, InWorldNormal );

	
	#ifdef VS_USES_UNREAL_SPACE
		UnrealWorldPos = ToUnrealPos( UnrealWorldPos );
	#endif
	Parameters.WorldPosition = UnrealWorldPos;
	#ifdef VS_USES_UNREAL_SPACE
		Parameters.TangentToWorld[ 0 ] = Parameters.TangentToWorld[ 0 ].xzy;
		Parameters.TangentToWorld[ 1 ] = Parameters.TangentToWorld[ 1 ].xzy;
		Parameters.TangentToWorld[ 2 ] = Parameters.TangentToWorld[ 2 ].xzy;//WorldAligned texturing uses normals that think Z is up
	#endif

	Parameters.VertexColor = VertexColor;

#if NUM_MATERIAL_TEXCOORDS_VERTEX > 0			
	Parameters.TexCoords[ 0 ] = float2( UV0.x, UV0.y );
#endif
#if NUM_MATERIAL_TEXCOORDS_VERTEX > 1
	Parameters.TexCoords[ 1 ] = float2( UV1.x, UV1.y );
#endif
#if NUM_MATERIAL_TEXCOORDS_VERTEX > 2
	for( int i = 2; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
	{
		Parameters.TexCoords[ i ] = float2( UV0.x, UV0.y );
	}
#endif

	Parameters.PrimitiveId = 0;

	SetupCommonData( Parameters.PrimitiveId );

#ifdef UE5
	Parameters.PrevFrameLocalToWorld = MakeLWCMatrix( float3( 0, 0, 0 ), Primitive.LocalToWorld );
#else
	Parameters.PrevFrameLocalToWorld = Primitive.LocalToWorld;
#endif
	
	float3 Offset = float3( 0, 0, 0 );
	Offset = GetMaterialWorldPositionOffset( Parameters );
	#ifdef VS_USES_UNREAL_SPACE
		//Convert from unreal units to unity
		Offset /= float3( 100, 100, 100 );
		Offset = Offset.xzy;
	#endif
	return Offset;
}

void SurfaceReplacement( Input In, out SurfaceOutputStandard o )
{
	InitializeExpressions();

	float3 Z3 = float3( 0, 0, 0 );
	float4 Z4 = float4( 0, 0, 0, 0 );

	float3 UnrealWorldPos = float3( In.worldPos.x, In.worldPos.y, In.worldPos.z );

	float3 UnrealNormal = In.normal2;	

	FMaterialPixelParameters Parameters = (FMaterialPixelParameters)0;
#if NUM_TEX_COORD_INTERPOLATORS > 0			
	Parameters.TexCoords[ 0 ] = float2( In.uv_MainTex.x, 1.0 - In.uv_MainTex.y );
#endif
#if NUM_TEX_COORD_INTERPOLATORS > 1
	Parameters.TexCoords[ 1 ] = float2( In.uv2_Material_Texture2D_0.x, 1.0 - In.uv2_Material_Texture2D_0.y );
#endif
#if NUM_TEX_COORD_INTERPOLATORS > 2
	for( int i = 2; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
	{
		Parameters.TexCoords[ i ] = float2( In.uv_MainTex.x, 1.0 - In.uv_MainTex.y );
	}
#endif
	Parameters.VertexColor = In.color;
	Parameters.WorldNormal = UnrealNormal;
	Parameters.ReflectionVector = half3( 0, 0, 1 );
	Parameters.CameraVector = normalize( _WorldSpaceCameraPos.xyz - UnrealWorldPos.xyz );
	//Parameters.CameraVector = mul( ( float3x3 )unity_CameraToWorld, float3( 0, 0, 1 ) ) * -1;
	Parameters.LightVector = half3( 0, 0, 0 );
	//float4 screenpos = In.screenPos;
	//screenpos /= screenpos.w;
	Parameters.SvPosition = In.screenPos;
	Parameters.ScreenPosition = Parameters.SvPosition;

	Parameters.UnMirrored = 1;

	Parameters.TwoSidedSign = 1;


	float3 InWorldNormal = UnrealNormal;	
	float4 tangentWorld = In.tangent;
	tangentWorld.xyz = normalize( tangentWorld.xyz );
	//float3x3 tangentToWorld = CreateTangentToWorldPerVertex( InWorldNormal, tangentWorld.xyz, tangentWorld.w );
	Parameters.TangentToWorld = float3x3( normalize( cross( InWorldNormal, tangentWorld.xyz ) * tangentWorld.w ), tangentWorld.xyz, InWorldNormal );

	//WorldAlignedTexturing in UE relies on the fact that coords there are 100x larger, prepare values for that
	//but watch out for any computation that might get skewed as a side effect
	UnrealWorldPos = ToUnrealPos( UnrealWorldPos );
	
	Parameters.AbsoluteWorldPosition = UnrealWorldPos;
	Parameters.WorldPosition_CamRelative = UnrealWorldPos;
	Parameters.WorldPosition_NoOffsets = UnrealWorldPos;

	Parameters.WorldPosition_NoOffsets_CamRelative = Parameters.WorldPosition_CamRelative;
	Parameters.LightingPositionOffset = float3( 0, 0, 0 );

	Parameters.AOMaterialMask = 0;

	Parameters.Particle.RelativeTime = 0;
	Parameters.Particle.MotionBlurFade;
	Parameters.Particle.Random = 0;
	Parameters.Particle.Velocity = half4( 1, 1, 1, 1 );
	Parameters.Particle.Color = half4( 1, 1, 1, 1 );
	Parameters.Particle.TranslatedWorldPositionAndSize = float4( UnrealWorldPos, 0 );
	Parameters.Particle.MacroUV = half4( 0, 0, 1, 1 );
	Parameters.Particle.DynamicParameter = half4( 0, 0, 0, 0 );
	Parameters.Particle.LocalToWorld = float4x4( Z4, Z4, Z4, Z4 );
	Parameters.Particle.Size = float2( 1, 1 );
	Parameters.Particle.SubUVCoords[ 0 ] = Parameters.Particle.SubUVCoords[ 1 ] = float2( 0, 0 );
	Parameters.Particle.SubUVLerp = 0.0;
	Parameters.TexCoordScalesParams = float2( 0, 0 );
	Parameters.PrimitiveId = 0;
	Parameters.VirtualTextureFeedback = 0;

	FPixelMaterialInputs PixelMaterialInputs = (FPixelMaterialInputs)0;
	PixelMaterialInputs.Normal = float3( 0, 0, 1 );
	PixelMaterialInputs.ShadingModel = 0;
	PixelMaterialInputs.FrontMaterial = 0;

	SetupCommonData( Parameters.PrimitiveId );
	//CustomizedUVs
	#if NUM_TEX_COORD_INTERPOLATORS > 0 && HAS_CUSTOMIZED_UVS
		float2 OutTexCoords[ NUM_TEX_COORD_INTERPOLATORS ];
		//Prevent uninitialized reads
		for( int i = 0; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
		{
			OutTexCoords[ i ] = float2( 0, 0 );
		}
		GetMaterialCustomizedUVs( Parameters, OutTexCoords );
		for( int i = 0; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
		{
			Parameters.TexCoords[ i ] = OutTexCoords[ i ];
		}
	#endif
	//<-
	CalcPixelMaterialInputs( Parameters, PixelMaterialInputs );

	#define HAS_WORLDSPACE_NORMAL 0
	#if HAS_WORLDSPACE_NORMAL
		PixelMaterialInputs.Normal = mul( PixelMaterialInputs.Normal, (MaterialFloat3x3)( transpose( Parameters.TangentToWorld ) ) );
	#endif

	o.Albedo = PixelMaterialInputs.BaseColor.rgb;
	o.Alpha = PixelMaterialInputs.Opacity;
	//if( PixelMaterialInputs.OpacityMask < 0.333 ) discard;

	o.Metallic = PixelMaterialInputs.Metallic;
	o.Smoothness = 1.0 - PixelMaterialInputs.Roughness;
	o.Normal = normalize( PixelMaterialInputs.Normal );
	o.Emission = PixelMaterialInputs.EmissiveColor.rgb;
	o.Occlusion = PixelMaterialInputs.AmbientOcclusion;

	//BLEND_ADDITIVE o.Alpha = ( o.Emission.r + o.Emission.g + o.Emission.b ) / 3;
}