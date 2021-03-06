import React, { Fragment, useEffect } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Spinner from '../layout/Spinner';
import ProfileActions from './ProfileActions';
import { getCurrentProfile } from '../../actions/profile';

const Profile = ({
  getCurrentProfile,
  auth: { user },
  profile: { profile, loading }
}) => {
  useEffect(() => {
    getCurrentProfile();
  }, [getCurrentProfile]);
  return loading || profile === null ? (
    <Fragment>
      <h1 className='large text-primary'>Profile</h1>
      <Spinner />
    </Fragment>
  ) : (
    <Fragment>
      <h1 className='large text-primary'>Profile</h1>
      <p className='lead'>
        <i className='fas fa-user' /> Welcome {user && user.first}
      </p>

      {profile !== null ? (
        <ProfileActions />
      ) : (
        <Fragment>
          <p>You have not yet setup a profile, please add some info</p>
          <Link to='/create-profile' className='btn btn-primary my-1'>
            Create Profile
          </Link>
        </Fragment>
      )}
    </Fragment>
  );
};

Profile.propTypes = {
  getCurrentProfile: PropTypes.func.isRequired,
  auth: PropTypes.object.isRequired,
  profile: PropTypes.object.isRequired
};

const mapStateToProps = state => ({
  auth: state.auth,
  profile: state.profile
});

export default connect(
  mapStateToProps,
  { getCurrentProfile }
)(Profile);
