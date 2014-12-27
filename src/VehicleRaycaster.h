#pragma once

namespace BulletSharp
{
	public ref class VehicleRaycasterResult
	{
	internal:
		btVehicleRaycaster::btVehicleRaycasterResult* _native;

	public:
		!VehicleRaycasterResult();
	protected:
		~VehicleRaycasterResult();

	public:
		VehicleRaycasterResult();

		property btScalar DistFraction
		{
			btScalar get();
			void set(btScalar value);
		}

		property Vector3 HitNormalInWorld
		{
			Vector3 get();
			void set(Vector3 value);
		}

		property Vector3 HitPointInWorld
		{
			Vector3 get();
			void set(Vector3 value);
		}
	};


	public interface class IVehicleRaycaster
	{
		virtual Object^ CastRay(Vector3 from, Vector3 to, VehicleRaycasterResult^ result);
	};


	public ref class VehicleRaycaster abstract : IVehicleRaycaster
	{
	internal:
		btVehicleRaycaster* _native;

		VehicleRaycaster(btVehicleRaycaster* native);

	public:
		!VehicleRaycaster();
	protected:
		~VehicleRaycaster();

	public:
		virtual Object^ CastRay(Vector3 from, Vector3 to, VehicleRaycasterResult^ result);
	};
};
